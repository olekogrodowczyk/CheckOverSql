using Application.Dto.CreateSolutionDto;
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

        public SolutionService(IMapper mapper, ISolutionRepository solutionRepository, IUserContextService userContextService
            ,IDatabaseQuery databaseQuery)
        {
            _mapper = mapper;
            _solutionRepository = solutionRepository;
            _userContextService = userContextService;
            _databaseQuery = databaseQuery;
        }

        public async Task<int> CreateSolutionAsync(CreateSolutionDto model, int exerciseId)
        {
            var solution = _mapper.Map<Solution>(model);
            solution.CreatorId = (int)_userContextService.GetUserId; 
            solution.ExerciseId= exerciseId;
            await _solutionRepository.Add(solution);
            await SendSolutionQuery(solution.Id);
            return solution.Id;
        }

        public async Task<Dictionary<int, object>> SendSolutionQuery(int solutionId)
        {
            var solution = await _solutionRepository.GetById(solutionId);
            var connectionString = await _solutionRepository.GetDatabaseConnectionString(solutionId);
            var result = await _databaseQuery.GetData(solution.Query, connectionString.Replace("\\\\","\\"));
            return result;
        }
    }
}
