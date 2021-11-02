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
        private readonly IDatabaseService _databaseService;

        public ExerciseService(IMapper mapper, IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IDatabaseRepository databaseRepository, IDatabaseService databaseService)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
        }
     
        public async Task<int> CreateExercise(CreateExerciseDto model)
        {
            await _databaseService.SendQueryNoData(model.ValidAnswer, model.Database);
            var exercise = _mapper.Map<Exercise>(model);         
            exercise.CreatorId = (int)_userContextService.GetUserId;  
            exercise.DatabaseId = await _databaseRepository.GetDatabaseIdByName(model.Database);
            await _exerciseRepository.Add(exercise);
            return exercise.Id;
        }

        public async Task<IEnumerable<GetExerciseVm>> GetAllExercisesCreatedByLoggedUser()
        {
            var exercises = await _exerciseRepository.GetAllInclude(x=>x.Creator);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseVm>>(exercises);
            return exerciseDtos;
        }

        public async Task<IEnumerable<GetExerciseVm>> GetAllPublicExercises()
        {
            var exercises = await _exerciseRepository.GetWhere(x => !x.IsPrivate);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseVm>>(exercises);
            return exerciseDtos;
        }
    }
}
