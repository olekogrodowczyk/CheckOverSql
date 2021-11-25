using Application.Common.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public class GetSolvingByIdRequirementHandler : AuthorizationHandler<GetSolvingByIdRequirement, Solving>
    {
        private readonly IUserContextService _userContextService;
        private readonly ISolvingRepository _solvingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public GetSolvingByIdRequirementHandler(IUserContextService userContextService, ISolvingRepository solvingRepository
            ,IUserRepository userRepository,  IAssignmentRepository assignmentRepository)
        {
            _userContextService = userContextService;
            _solvingRepository = solvingRepository;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GetSolvingByIdRequirement requirement, Solving solving)
        {
            int userId = (int)_userContextService.GetUserId;
            var userRoleName = await _userRepository.GetRoleName(userId);
            if (userId == solving.CreatorId || userRoleName == "Admin" || userId == solving.CreatorId)
            {
                context.Succeed(requirement);
                return;
            }

            var solvingsToCheck = await _solvingRepository.GetAllSolvingsAvailable(userId);

            bool ifExists = solvingsToCheck.Any(x => x.Id == solving.Id);
            if (!ifExists)
            {
                throw new ForbidException("You don't have permission to get this solution", true);
            }                 
        }
    }
}
