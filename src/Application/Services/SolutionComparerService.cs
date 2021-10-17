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
    public class SolutionComparerService : ISolutionComparerService
    {
        private readonly IDatabaseQuery _databaseQuery;
        private readonly ISolutionRepository _solutionRepository;

        public SolutionComparerService(IDatabaseQuery databaseQuery, ISolutionRepository solutionRepository)           
        {
            _databaseQuery = databaseQuery;
            _solutionRepository = solutionRepository;
        }

        public async Task<bool> Compare(Solution solution1, Solution solution2)
        {
            var solutionOutcome1 =
                await _databaseQuery.GetData(solution1.Query, await _solutionRepository.GetDatabaseConnectionString(solution1.Id));
            var solutionOutcome2 =
                await _databaseQuery.GetData(solution2.Query, await _solutionRepository.GetDatabaseConnectionString(solution2.Id));
            return true;
        }
    }
}
