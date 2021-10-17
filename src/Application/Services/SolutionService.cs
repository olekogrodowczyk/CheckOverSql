﻿using Application.Dto.CreateSolutionDto;
using Application.Interfaces;
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

        public SolutionService(IMapper mapper, ISolutionRepository solutionRepository, IUserContextService userContextService
            ,IDatabaseQuery databaseQuery, IExerciseRepository exerciseRepository)
        {
            _mapper = mapper;
            _solutionRepository = solutionRepository;
            _userContextService = userContextService;
            _databaseQuery = databaseQuery;
            _exerciseRepository = exerciseRepository;
        }

        public async Task<int> CreateSolutionAsync(CreateSolutionDto model, int exerciseId)
        {
            var solution = _mapper.Map<Solution>(model);
            solution.CreatorId = (int)_userContextService.GetUserId; 
            solution.ExerciseId= exerciseId;
            await _solutionRepository.Add(solution);
            return solution.Id;
        }

        public async Task<Dictionary<int, object>> SendSolutionQueryAsync(int solutionId)
        {
            var solution = await _solutionRepository.GetById(solutionId);
            var result = await sendQueryAsync(solution.Query, await _solutionRepository.GetDatabaseConnectionString(solutionId));
            return result;
        }
    
        public async Task<bool> CompareAsync(int solutionId, int exerciseId)
        {
            var solution = await _solutionRepository.GetById(solutionId);
            var exercise = await _exerciseRepository.GetById(exerciseId);
            string connectionString = await _solutionRepository.GetDatabaseConnectionString(solutionId);

            var dict1 = await sendQueryAsync(solution.Query, connectionString);
            var dict2 = await sendQueryAsync(exercise.ValidAnswer, connectionString);

            var result = compareDictionaries(dict1, dict2);

            return result;
        }

        private async Task<Dictionary<int, object>> sendQueryAsync(string query, string connectionString)
        {
            var result = await _databaseQuery.GetData(query, connectionString.Replace("\\\\", "\\"));
            return result;
        }

        private bool compareDictionaries(Dictionary<int, object> dict1, Dictionary<int, object> dict2)
        {
            if (dict1.Count != dict2.Count)
            {
                return false;
            }
            for (int i = 0; i < dict1.Count; i++)
            {
                if (!dict1[i].Equals(dict2[i]))
                {
                    return false;
                }
            }
            return true;
        }


    }
}
