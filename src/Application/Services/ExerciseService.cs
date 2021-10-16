using Application.Dto.CreateExerciseDto;
using Application.Dto.GetExerciseDto;
using Application.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseQuery _databaseQuery;

        public ExerciseService(IMapper mapper, IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IConfiguration configuration, IDatabaseQuery databaseQuery)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _configuration = configuration;
            _databaseQuery = databaseQuery;
        }

        public async Task<int> CreateExerciseAsync(CreateExerciseDto model)
        {
            var exercise = _mapper.Map<Exercise>(model);
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is not null)
            {
                exercise.CreatorId = (int)_userContextService.GetUserId;
            }            
            await _exerciseRepository.Add(exercise);
            return exercise.Id;
        }


        public async Task<IEnumerable<GetExerciseDto>> GetAllExercisesAsync()
        {
            await _databaseQuery.GetData("SELECT * FROM dbo.Footballers", ExerciseDatabaseEnum.FootballLeague);
            var exercises = await _exerciseRepository.GetAllInclude(x=>x.Creator);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseDto>>(exercises);
            return exerciseDtos;
        }
    }
}
