using Application.Interfaces;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GroupRoles.Commands
{
    public class ChangeRoleCommandValidator : AbstractValidator<ChangeRoleCommand>
    {
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IUserContextService _userContextService;

        public ChangeRoleCommandValidator(IGroupRoleRepository groupRoleRepository, IUserContextService userContextService)
        {
            _groupRoleRepository = groupRoleRepository;
            _userContextService = userContextService;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("Specified user cannot be empty")
                .MustAsync(CheckForCreator).WithMessage("Creators cannot change their roles");

            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("Specified group cannot be empty");

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Specified role name cannot be empty");
        }

        public async Task<bool> CheckForCreator(int userId, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            if ((int)loggedUserId == userId) { return false; }
            return true;
        }


    }
}
