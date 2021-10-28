using Application.Dto.CreateExerciseDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
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
        private readonly IDatabaseRepository _databaseRepository;

        public ExerciseService(IMapper mapper, IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IDatabaseRepository databaseRepository)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _databaseRepository = databaseRepository;
        }

        public async Task<int> CreateExerciseAsync(CreateExerciseDto model)
        {
            var exercise = _mapper.Map<Exercise>(model);         
            exercise.CreatorId = (int)_userContextService.GetUserId;  
            exercise.DatabaseId = await _databaseRepository.GetDatabaseIdByName(model.Database);
            await _exerciseRepository.Add(exercise);
            return exercise.Id;
        }


        public async Task<IEnumerable<GetExerciseVm>> GetAllExercisesAsync()
        {
            //await _databaseQuery.GetData("SELECT * FROM dbo.Footballers", ExerciseDatabaseEnum.FootballLeague);
            var exercises = await _exerciseRepository.GetAllInclude(x=>x.Creator);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseVm>>(exercises);
            return exerciseDtos;
        }
    }
}
