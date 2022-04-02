using Application.Common.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            , IUserRepository userRepository, IAssignmentRepository assignmentRepository)
        {
            _userContextService = userContextService;
            _solvingRepository = solvingRepository;
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GetSolvingByIdRequirement requirement, Solving solving)
        {
            if (context is null || context.User.Claims.Count() == 0) { throw new UnauthorizedAccessException(); }
            var userId = int.Parse(context.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var userRoleName = await _userRepository.GetRoleName(userId);
            if (userId == solving.CreatorId || userRoleName == "Admin")
            {
                context.Succeed(requirement);
                return;
            }


            context.Fail();
        }
    }
}
