using Application.Authorization;
using Application.Interfaces;
using Application.ViewModels;
using AutoMapper;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SolvingService : ISolvingService
    {
        private readonly ISolutionRepository _solutionRepository;
        private readonly ISolvingRepository _solvingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IAssignmentRepository _assignmentRepository;

        public SolvingService(ISolutionRepository solutionRepository, ISolvingRepository solvingRepository
            ,IUserContextService userContextService, IMapper mapper, IAuthorizationService authorizationService
            ,IAssignmentRepository assignmentRepository)
        {
            _solutionRepository = solutionRepository;
            _solvingRepository = solvingRepository;
            _userContextService = userContextService;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<IEnumerable<GetSolvingVm>> GetAllSolvingsAssignedToUser()
        {
            var userId = _userContextService.GetUserId;
            var solvings = await _solvingRepository.GetSolvingsAssignedToUserToDo((int)userId);
            var solvingsVm = _mapper.Map<IEnumerable<GetSolvingVm>>(solvings);
            return solvingsVm;
        }

        public async Task<GetSolvingVm> GetSolvingById(int solvingId)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var solving = await _solvingRepository.GetSolvingWithIncludes(solvingId);
            var authorizationGetSolvingByIdResult = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, solving, new GetSolvingByIdRequirement());

            if (!authorizationGetSolvingByIdResult.Succeeded)
            {
                //Solving exists and now we should check the permission
                var loggedUserAssignment =
                await _assignmentRepository.GetUserAssignmentBasedOnOtherAssignment(loggedUserId, solving.AssignmentId);

                string permission = GetPermissionByEnum.GetPermissionName(PermissionNames.CheckingExercises);
                var authorizationPermissionRequirement = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, loggedUserAssignment, new PermissionRequirement(permission));
            }
         
            var solvingDto = _mapper.Map<GetSolvingVm>(solving);
            return solvingDto;
        }
    }
}
