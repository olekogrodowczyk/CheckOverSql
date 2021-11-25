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

        public CreateSolutionCommandHandler(IMapper mapper, IExerciseRepository exerciseRepository
            ,IUserContextService userContextService, ISolutionRepository solutionRepository
            ,IDatabaseRepository databaseRepository, IDatabaseService databaseService
            ,ISolutionService solutionService, IComparisonRepository comparisonRepository)
        {
            _mapper = mapper;
            _exerciseRepository = exerciseRepository;
            _userContextService = userContextService;
            _solutionRepository = solutionRepository;
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
            _solutionService = solutionService;
            _comparisonRepository = comparisonRepository;
        }

        public async Task<GetComparisonDto> Handle(CreateSolutionCommand command, CancellationToken cancellationToken)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(command.ExerciseId);

            Solution solution = _mapper.Map<Solution>(command);
            solution.Creator = _userContextService.User;
            await _solutionRepository.AddAsync(solution);

            string databaseName = await _databaseRepository.GetDatabaseNameById(exercise.DatabaseId);
            solution.Checked = true; solution.IsValid = false;
            await _databaseService.SendQueryNoData(command.Query, databaseName);
            solution.IsValid = true;

            Comparison comparison = await _solutionService.CreateComparison(solution.Id, command.ExerciseId);
            if (comparison.Result) { await _solutionService.HandlePossibleSolvingToDo(command.ExerciseId, solution); }

            comparison = await _comparisonRepository.GetComparisonWithIncludes(comparison.Id);
            var comparisonDto = _mapper.Map<GetComparisonDto>(comparison);

            return comparisonDto;
        }
    }
}
