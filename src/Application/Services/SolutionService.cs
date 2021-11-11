﻿using Application.Dto.CreateSolutionDto;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
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
        private readonly IDatabaseService _queryService;
        private readonly IDataComparerService _dataComparer;
        private readonly IComparisonRepository _comparisonRepository;

        public SolutionService(IMapper mapper, ISolutionRepository solutionRepository, IUserContextService userContextService
            ,IDatabaseQuery databaseQuery, IExerciseRepository exerciseRepository, IDatabaseService queryService
            ,IDataComparerService dataComparer, IComparisonRepository comparisonRepository)
        {
            _mapper = mapper;
            _solutionRepository = solutionRepository;
            _userContextService = userContextService;
            _databaseQuery = databaseQuery;
            _exerciseRepository = exerciseRepository;
            _queryService = queryService;
            _dataComparer = dataComparer;
            _comparisonRepository = comparisonRepository;
        }

        public async Task<int> CreateSolution(CreateSolutionDto model, int exerciseId)
        {
            await _exerciseRepository.GetByIdAsync(exerciseId);
            var solution = _mapper.Map<Solution>(model);          
            solution.CreatorId = (int)_userContextService.GetUserId; 
            solution.ExerciseId= exerciseId;
            await _solutionRepository.AddAsync(solution);
            return solution.Id;
        }

        public async Task<List<List<string>>> SendSolutionQuery(int solutionId)
        {
            var solution = await _solutionRepository.GetByIdAsync(solutionId);
            string databaseName = await _solutionRepository.GetDatabaseName(solutionId);
            var result = await _queryService.SendQueryWithData(solution.Query, databaseName);
            return result;
        }
    
        public async Task<bool> Compare(int solutionId, int exerciseId)
        {
            var solution = await _solutionRepository.GetByIdAsync(solutionId);
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            string databaseName = await _solutionRepository.GetDatabaseName(solutionId);

            var list1 = await _queryService.SendQueryWithData(solution.Query, databaseName);
            var list2 = await _queryService.SendQueryWithData(exercise.ValidAnswer, databaseName);

            var result = await _dataComparer.compareValues(list1, list2);

            return result;
        }

        public async Task CreateComparison(int solutionId, int exerciseId, bool comparisonResult)
        {
            var solution = await _solutionRepository.GetByIdAsync(solutionId);
            var exercise = await _exerciseRepository.GetByIdAsync(exerciseId);
            Comparison comparison = new Comparison
            {
                Solution = solution,
                SolutionId = solutionId,
                Exercise = exercise,
                ExerciseId = exerciseId,
                Result = comparisonResult,
            };
            await _comparisonRepository.AddAsync(comparison);
        }

        public async Task<IEnumerable<GetSolutionVm>> GetAllSolutions(int exerciseId)
        {
            var solutions = await _solutionRepository.GetAllCreatedByUser(exerciseId);
            var solutionsVm = _mapper.Map<IEnumerable<GetSolutionVm>>(solutions);
            return solutionsVm;
        }

    }
}
