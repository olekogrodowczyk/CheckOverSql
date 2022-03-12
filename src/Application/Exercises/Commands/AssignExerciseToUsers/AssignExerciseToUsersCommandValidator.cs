using Application.Authorization;
using Application.Common.Exceptions;
using Application.Dto.AssignExerciseToUsersTo;
using Application.Interfaces;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Dto.AssignExerciseToUsers
{
    public class AssignExerciseToUsersCommandValidator : AbstractValidator<AssignExerciseToUsersCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public AssignExerciseToUsersCommandValidator(IGroupRepository groupRepository, IAssignmentRepository assignmentRepository
            , IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _groupRepository = groupRepository;
            _assignmentRepository = assignmentRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;

            RuleFor(x => x.DeadLine)
                .GreaterThanOrEqualTo(DateTime.UtcNow.AddHours(1))
                .WithMessage("Defined date is invalid, deadline can be only set one hour later from now");

            RuleFor(x => x.GroupId)
                .MustAsync(CheckIfUserCanAssignExerciseToUsers)
                .WithMessage("You don't belong to the group or don't have permission");

        }

        public async Task<bool> CheckIfUserCanAssignExerciseToUsers(int groupId, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group is null) { throw new NotFoundException(nameof(group), groupId); }

            var authorizationResult = await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal,
                group, new PermissionRequirement(PermissionEnum.AssigningExercises));
            if (!authorizationResult.Succeeded) { return false; }

            return true;
        }
    }


}
