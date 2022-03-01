using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Invitations.Commands.CreateInvitation;
using Domain.Enums;
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
    public class CreateInvitationCommandValidator : AbstractValidator<CreateInvitationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private IEnumerable<string> groupRoleNames;

        public CreateInvitationCommandValidator(IUserRepository userRepository, IGroupRoleRepository groupRoleRepository
            ,IInvitationRepository invitationRepository, IUserContextService userContextService
            ,IAssignmentRepository assignmentRepository)
        {
            _userRepository = userRepository;
            _groupRoleRepository = groupRoleRepository;
            _invitationRepository = invitationRepository;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            groupRoleNames = getGroupRoleNames();

            CascadeMode = CascadeMode.Stop;

            RuleFor(r => r.RoleName)
                .NotEmpty().WithMessage("Role hasn't been specified")
                .Must(GroupRoleExists).WithMessage($"Role should be in {string.Join(',', groupRoleNames)}");

            RuleFor(r => r.ReceiverEmail)
                .NotEmpty().WithMessage("Receiver email hasn't been specified")
                .MustAsync(EmailExists).WithMessage("Defined email doesn't exist");

            RuleFor(g => g.GroupId).MustAsync
                (async (model, groupId, cancellation) =>
                {
                    return await CheckIfInvitationAlreadyExists(model.ReceiverEmail, model.RoleName, groupId, cancellation);
                }).WithMessage("Similar invitation already exists").MustAsync
                (async (model, groupId, cancellation) =>
                {
                    return await CheckIfUserIsAlreadyInGroup(model.ReceiverEmail, model.RoleName, groupId, cancellation);
                }).WithMessage("User already is in the group")
                .MustAsync(CheckIfSenderIsInTheGroup).WithMessage("You are not in the group");


        }

        public IEnumerable<string> getGroupRoleNames()
        {
            var groupRoles = _groupRoleRepository.GetAllAsync().Result;
            return groupRoleNames = groupRoles.Select(x => x.Name);
        }

        public bool GroupRoleExists(string roleName)
        {         
            return groupRoleNames.Contains(roleName);
        }

        public async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
        {
            var result = await _userRepository.SingleOrDefaultAsync(x => x.Email == email);
            if(result is not null) { return true; }
            return false;
        }

        public async Task<bool> CheckIfInvitationAlreadyExists(string email, string role, int groupId, CancellationToken cancellationToken)
        {
            var receiver = await _userRepository.GetByEmail(email);
            var groupRole = await _groupRoleRepository.GetByName(role);

            bool result = await _invitationRepository
                .AnyAsync(x => x.ReceiverId == receiver.Id && x.Status == InvitationStatusEnum.Sent
                && x.GroupId == groupId && x.GroupRoleId == groupRole.Id);

            if (!result) { return true; }
            return false;
        }

        public async Task<bool> CheckIfUserIsAlreadyInGroup(string email, string role, int groupId, CancellationToken cancellationToken)
        {
            var receiver = await _userRepository.GetByEmail(email);
            bool result = await _assignmentRepository.AnyAsync(x => x.UserId == receiver.Id && x.GroupId == groupId);

            if (!result) { return true; }
            return false;
        }

        public async Task<bool> CheckIfSenderIsInTheGroup(int groupId, CancellationToken cancellationToken)
        {
            int senderId = (int)_userContextService.GetUserId;
            var result = await _assignmentRepository.AnyAsync(x => x.GroupId == groupId && x.UserId == senderId);

            if (result) { return true; }
            return false;
        }
    }
}
