using Application.Interfaces;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Dto.CreateInvitationDto
{
    public class CreateInvitationDtoValidator : AbstractValidator<CreateInvitationDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContextService _userContextService;
        private IEnumerable<string> groupRoleNames;

        public CreateInvitationDtoValidator(IUserRepository userRepository, IGroupRoleRepository groupRoleRepository
            ,IInvitationRepository invitationRepository, IUserContextService userContextService)
        {
            _userRepository = userRepository;
            _groupRoleRepository = groupRoleRepository;
            _invitationRepository = invitationRepository;
            _userContextService = userContextService;
            groupRoleNames = getGroupRoleNames();

            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage("Role hasn't been specified")
                .Must(GroupRoleExists).WithMessage($"Role should be in {string.Join(',', groupRoleNames)}");

            RuleFor(x => x.ReceiverEmail)
                .NotEmpty().WithMessage("Receiver email hasn't been specified")
                .MustAsync(EmailExists).WithMessage("Defined email doesn't exist");
        }

        public IEnumerable<string> getGroupRoleNames()
        {
            var groupRoles = _groupRoleRepository.GetAll().Result;
            return groupRoleNames = groupRoles.Select(x => x.Name);
        }

        public bool GroupRoleExists(string roleName)
        {         
            return groupRoleNames.Contains(roleName);
        }

        public async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
        {
            return await _userRepository.Exists(x => x.Email == email);
        }

        

    }
}
