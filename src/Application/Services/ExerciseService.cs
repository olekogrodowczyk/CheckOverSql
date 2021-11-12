using Application.Authorization;
using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ISolvingRepository _solvingRepository;
        private readonly IGroupRepository _groupRepository;

        public ExerciseService(IMapper mapper, IUserContextService userContextService, IExerciseRepository exerciseRepository
            ,IDatabaseRepository databaseRepository, IDatabaseService databaseService, IAssignmentRepository assignmentRepository
            ,IAuthorizationService authorizationService, ISolvingRepository solvingRepository, IGroupRepository groupRepository)
        {
            _mapper = mapper;
            _userContextService = userContextService;
            _exerciseRepository = exerciseRepository;
            _databaseRepository = databaseRepository;
            _databaseService = databaseService;
            _assignmentRepository = assignmentRepository;
            _authorizationService = authorizationService;
            _solvingRepository = solvingRepository;
            _groupRepository = groupRepository;
        }
     
        public async Task<int> CreateExercise(CreateExerciseDto model)
        {
            await _databaseService.SendQueryNoData(model.ValidAnswer, model.Database);
            var exercise = _mapper.Map<Exercise>(model);         
            exercise.CreatorId = (int)_userContextService.GetUserId;  
            exercise.DatabaseId = await _databaseRepository.GetDatabaseIdByName(model.Database);
            await _exerciseRepository.AddAsync(exercise);
            return exercise.Id;
        }

        public async Task<IEnumerable<GetExerciseVm>> GetAllExercisesCreatedByLoggedUser()
        {
            var exercises = await _exerciseRepository.GetAllIncludeAsync(x=>x.Creator);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseVm>>(exercises);
            return exerciseDtos;
        }

        public async Task<IEnumerable<GetExerciseVm>> GetAllPublicExercises()
        {
            var exercises = await _exerciseRepository.GetWhereAsync(x => !x.IsPrivate);
            var exerciseDtos = _mapper.Map<IEnumerable<GetExerciseVm>>(exercises);
            return exerciseDtos;
        }

        public async Task<IEnumerable<int>> AssignExerciseToAllUsers(int groupId, int exerciseId, AssignExerciseToUsersDto model)
        {          
            var assignmentsChosenToGetExercise =
                await _assignmentRepository
                .GetWhereIncludeAsync(x => x.GroupId == groupId && x.GroupRole.Name == "User", x=>x.GroupRole);

            var solvingsIds = new List<int>();
            foreach (Assignment assignment in assignmentsChosenToGetExercise)
            {
                var solving = new Solving
                {
                    ExerciseId = exerciseId,
                    Status = SolvingStatus.ToDo.ToString(),
                    DeadLine = model.DeadLine,
                    AssignmentId = assignment.Id,
                    CreatorId = (int)_userContextService.GetUserId
                };
                await _solvingRepository.AddAsync(solving);
                solvingsIds.Add(solving.Id);
            }
            return solvingsIds;
        }

        public async Task CheckIfUserCanAssignExerciseToUsers(int groupId)
        {
            bool groupExists = await _groupRepository.ExistsAsync(x => true);
            if(!groupExists) { throw new NotFoundException($"Defined group with id: {groupId} doesn't exist", true); }

            var assignment = await _assignmentRepository
                .SingleAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == groupId);
            if (assignment == null)
            {
                throw new NotFoundException("You don't belong to the specified group or the group doesn't exist");
            }

            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal, assignment
                ,new PermissionRequirement("Assigning exercises"));
        }


    }
}
