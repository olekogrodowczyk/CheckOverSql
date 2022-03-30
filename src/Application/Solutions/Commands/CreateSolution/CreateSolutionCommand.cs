using Application.Interfaces;
using Application.Groups;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Mappings;
using Application.Common.Exceptions;
using Domain.Enums;

namespace Application.Solutions.Commands.CreateSolution
{
    public class CreateSolutionCommand : IMap, IRequest<GetComparisonDto>
    {
        public string Query { get; set; }
        public int ExerciseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSolutionCommand, Solution>();
        }
    }

    public class CreateSolutionCommandHandler : IRequestHandler<CreateSolutionCommand, GetComparisonDto>
    {
        private readonly IMapper _mapper;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserContextService _userContextService;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IDatabaseRepository _databaseRepository;
        private readonly IDatabaseService _databaseService;
        private readonly ISolutionService _solutionService;
        private readonly IComparisonRepository _comparisonRepository;
        private readonly ISolvingRepository _solvingRepository;

        public CreateSolutionCommandHandler(IMapper mapper, IExerciseRepository exerciseRepository
            , IUserContextService userContextService, ISolutionRepository solutionRepository
            , IDatabaseRepository databaseRepository, IDatabaseService databaseService
            , ISolutionService solutionService, IComparisonRepository comparisonRepository
            , ISolvingRepository solvingRepository)
        {
            _mapper = mapper;
            _exerciseRepository = exerciseRepository;
            _userContextService = userContextService;
            _solutionRepository = solutionRepository;
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
            _solutionService = solutionService;
            _comparisonRepository = comparisonRepository;
            _solvingRepository = solvingRepository;
        }

        public async Task<GetComparisonDto> Handle(CreateSolutionCommand command, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(command.ExerciseId);
            if (exercise.DatabaseId is null) { throw new NotFoundException(nameof(exercise), command.ExerciseId); }

            Solution solution = _mapper.Map<Solution>(command);
            solution.Creator = _userContextService.User;
            await _solutionRepository.AddAsync(solution);
            await validateSolution(command, exercise, solution);

            Comparison comparison = await createComparison(command, solution);

            comparison = await _comparisonRepository.GetComparisonWithIncludes(comparison.Id);
            var comparisonDto = _mapper.Map<GetComparisonDto>(comparison);

            return comparisonDto;
        }

        private async Task validateSolution(CreateSolutionCommand command, Exercise exercise, Solution solution)
        {
            string databaseName = await _databaseRepository.GetDatabaseNameById((int)exercise.DatabaseId);
            solution.IsValid = false;
            await _solutionRepository.UpdateAsync(solution);
            await _databaseService.ValidateQuery(command.Query, databaseName);
            solution.IsValid = true;
        }

        private async Task<Comparison> createComparison(CreateSolutionCommand command, Solution solution)
        {
            Comparison comparison = await _solutionService.CreateComparison(solution.Id, command.ExerciseId);
            if (comparison.Result)
            {
                await handlePossibleSolvingToDo(command.ExerciseId, solution);
                solution.Outcome = true;
                await _solutionRepository.UpdateAsync(solution);
            }

            return comparison;
        }

        private async Task handlePossibleSolvingToDo(int exerciseId, Solution solution)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var solvingsAssignedToUser = await _solvingRepository.GetSolvingsAssignedToUserToDo(loggedUserId);
            foreach (var solvingAssignedToUser in solvingsAssignedToUser.Where(x => x.ExerciseId == exerciseId))
            {
                solvingAssignedToUser.Status = solvingAssignedToUser.DeadLine > DateTime.UtcNow ? SolvingStatusEnum.Done
                    : SolvingStatusEnum.DoneButOverdue;
                solvingAssignedToUser.SentAt = DateTime.UtcNow;
                solvingAssignedToUser.SolutionId = solution.Id;
                solution.Outcome = true;

                await _solutionRepository.UpdateAsync(solution);
                await _solvingRepository.UpdateAsync(solvingAssignedToUser);
            }
        }
    }
}
