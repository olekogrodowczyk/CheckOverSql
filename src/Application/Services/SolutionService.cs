using Application.Dto.CreateSolutionDto;
using Application.Dto.SendQueryDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SolutionService : ISolutionService
    {
        private readonly IMapper _mapper;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IUserContextService _userContextService;
        private readonly IDatabaseQuery _databaseQuery;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IDatabaseService _databaseService;
        private readonly IDataComparerService _dataComparer;
        private readonly IComparisonRepository _comparisonRepository;
        private readonly ISolvingRepository _solvingRepository;
        private readonly IDatabaseRepository _databaseRepository;

        public SolutionService(IMapper mapper, ISolutionRepository solutionRepository, IUserContextService userContextService
            ,IDatabaseQuery databaseQuery, IExerciseRepository exerciseRepository, IDatabaseService databaseService
            ,IDataComparerService dataComparer, IComparisonRepository comparisonRepository, ISolvingRepository solvingRepository
            ,IDatabaseRepository databaseRepository)
        {
            _mapper = mapper;
            _solutionRepository = solutionRepository;
            _userContextService = userContextService;
            _databaseQuery = databaseQuery;
            _exerciseRepository = exerciseRepository;
            _databaseService = databaseService;
            _dataComparer = dataComparer;
            _comparisonRepository = comparisonRepository;
            _solvingRepository = solvingRepository;
            _databaseRepository = databaseRepository;
        }

        public async Task<GetComparisonVm> CreateSolution(int exerciseId, CreateSolutionDto model)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);

            Solution solution = _mapper.Map<Solution>(model);
            solution.Creator = _userContextService.User;
            solution.ExerciseId = exerciseId;
            await _solutionRepository.AddAsync(solution);

            string databaseName = await _databaseRepository.GetDatabaseNameById(exercise.DatabaseId);
            solution.Checked = true; solution.IsValid = false;
            await _databaseService.SendQueryNoData(model.Query, databaseName);
            solution.IsValid = true;

            Comparison comparison = await CreateComparison(solution.Id, exerciseId);
            if (comparison.Result) { await HandlePossibleSolvingToDo(exerciseId, solution); }

            comparison = await _comparisonRepository.GetComparisonWithIncludes(comparison.Id);
            var comparisonVm = _mapper.Map<GetComparisonVm>(comparison);
                      
            return comparisonVm;
        }


        private async Task HandlePossibleSolvingToDo(int exerciseId, Solution solution)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var solvingsAssignedToUser = await _solvingRepository.GetSolvingsAssignedToUserToDo(loggedUserId);

            Solving solvingAssignedToUser = solvingsAssignedToUser.FirstOrDefault(x => x.ExerciseId == exerciseId);
            if (solvingAssignedToUser is not null)
            {
                solvingAssignedToUser.Status = solvingAssignedToUser.DeadLine > DateTime.UtcNow ? SolvingStatus.Done.ToString()
                    : SolvingStatus.DoneButOverdue.ToString();
                solvingAssignedToUser.SentAt = DateTime.UtcNow;
                solution.SolvingId = solvingAssignedToUser.Id;
                solution.Outcome = true;

                await _solutionRepository.UpdateAsync(solution);
                await _solvingRepository.UpdateAsync(solvingAssignedToUser);
            }
        }

        public async Task<List<List<string>>> SendSolutionQuery(int solutionId)
        {
            var solution = await _solutionRepository.GetByIdAsync(solutionId);
            string databaseName = await _solutionRepository.GetDatabaseName(solutionId);
            var result = await _databaseService.SendQueryWithData(solution.Query, databaseName);
            return result;
        }
    
        public async Task<bool> Compare(int exerciseId, string query)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            string databaseName = await _databaseRepository.GetDatabaseNameById(exercise.DatabaseId);

            var list1 = await _databaseService.SendQueryWithData(query, databaseName);
            var list2 = await _databaseService.SendQueryWithData(exercise.ValidAnswer, databaseName);

            bool result = await _dataComparer.compareValues(list1, list2);
            return result;
        }

        public async Task<Comparison> CreateComparison(int solutionId, int exerciseId)
        {
            var solution = await _solutionRepository.GetByIdAsync(solutionId);
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            var comparisonResult = await Compare(exerciseId, solution.Query);

            Comparison comparison = new Comparison
            {
                SolutionId = solutionId,
                ExerciseId = exerciseId,
                Result = comparisonResult,
            };
            await _comparisonRepository.AddAsync(comparison);
            return comparison;
        }

        public async Task<IEnumerable<GetSolutionVm>> GetAllSolutions(int exerciseId)
        {
            var solutions = await _solutionRepository.GetAllCreatedByUser(exerciseId);
            var solutionsVm = _mapper.Map<IEnumerable<GetSolutionVm>>(solutions);
            return solutionsVm;
        }

        public async Task<int> GetComparisonResult(int exerciseId, int solutionId)
        {
            var comparison = await _comparisonRepository.SingleAsync(x=>x.ExerciseId == exerciseId && x.SolutionId == solutionId);
            return comparison.Id;
        }

    }
}
