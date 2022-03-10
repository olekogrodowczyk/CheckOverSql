using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.GroupRoles.Queries.CheckPermission
{
    public class CheckPermissionQueryValidator : AbstractValidator<CheckPermissionQuery>
    {
        private readonly IGroupRepository _groupRepository;

        public CheckPermissionQueryValidator(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;

            RuleFor(x => x.Permission)
                .NotEmpty().WithMessage("Permission hasn't been specified")
                .Must(checkPermission).WithMessage($"Permission name doesn't exist" +
                $", available statuses: {string.Join(',', Enum.GetNames(typeof(PermissionEnum)))}");

            RuleFor(x => x.GroupId)
                .MustAsync(groupExists)
                .When(x => x.GroupId != null)
                .WithMessage("Specified group doesn't exist");
        }

        private bool checkPermission(string value)
        {
            PermissionEnum permissionEnum;
            if (Enum.TryParse(value, out permissionEnum)) { return true; }
            return false;
        }

        private async Task<bool> groupExists(int? groupId, CancellationToken cancellationToken)
        {
            if (groupId is null) { throw new ArgumentException(); }
            return await _groupRepository.AnyAsync(x => x.Id == (int)groupId);
        }
    }
}
