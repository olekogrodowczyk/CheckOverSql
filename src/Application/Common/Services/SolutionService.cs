using Application.Interfaces;
using Application.Solutions.Queries;
using Application.Groups;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;

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

        


        public async Task HandlePossibleSolvingToDo(int exerciseId, Solution solution)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var solvingsAssignedToUser = await _solvingRepository.GetSolvingsAssignedToUserToDo(loggedUserId);
            foreach (var solvingAssignedToUser in solvingsAssignedToUser.Where(x=>x.ExerciseId == exerciseId))
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

        
    
        public async Task<bool> Compare(int exerciseId, string query)
        {
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            if(exercise.DatabaseId is null) { throw new NotFoundException("Defined exercises doesn't have an assigned database"); }
            string databaseName = await _databaseRepository.GetDatabaseNameById((int)exercise.DatabaseId);

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

        public async Task<bool> CheckIfUserPassedExercise(int exerciseId)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var result = await _comparisonRepository.FirstOrDefaultAsync
                (x => x.Solution.CreatorId == (int)loggedUserId && x.ExerciseId == exerciseId && x.Result == true, x=>x.Solution);
            if(result is not null) { return true; }
            return false;            
        }

    }
}
