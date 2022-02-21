using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetUserGroupRole
{
    public class GetUserGroupRoleQueryValidator : AbstractValidator<GetUserGroupRoleQuery>
    {
        private readonly IGroupRepository _groupRepository;

        public GetUserGroupRoleQueryValidator(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;

            RuleFor(x => x.GroupId)
                .MustAsync(GroupExists).WithMessage("Defined group doesn't exist");
            
        }

        public async Task<bool> GroupExists(int value, CancellationToken cancellationToken)
        {
            return await _groupRepository.AnyAsync(x => x.Id == value);
        }
    }
}
