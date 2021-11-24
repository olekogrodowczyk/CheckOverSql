using Application.Authorization;
using Application.Common.Exceptions;
using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Interfaces;
using Application.Groups;
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

        public async Task CheckIfUserCanAssignExerciseToUsers(int groupId)
        {
            bool groupExists = await _groupRepository.AnyAsync(x => true);
            if(!groupExists) { throw new NotFoundException($"Defined group with id: {groupId} doesn't exist", true); }

            var assignment = await _assignmentRepository
                .SingleAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == groupId);
            if (assignment == null)
            {
                throw new NotFoundException("You don't belong to the specified group or the group doesn't exist");
            }

            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal, assignment
                ,new PermissionRequirement(PermissionNames.AssigningExercises));
        }


    }
}
