using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands.LeaveGroup
{
    public class LeaveGroupCommandValidator : AbstractValidator<LeaveGroupCommand>
    {
        private readonly IUserContextService _userContextService;
        private readonly IGroupRepository _groupRepository;

        public LeaveGroupCommandValidator(IUserContextService userContextService, IGroupRepository groupRepository)
        {
            _userContextService = userContextService;
            _groupRepository = groupRepository;

            RuleFor(x => x.GroupId)
                .NotEmpty().WithMessage("Specified group cannot be empty");
        }



    }
}
