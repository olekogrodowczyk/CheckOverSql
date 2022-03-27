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
        private readonly IGroupRepository _groupRepository;
        private IEnumerable<string> groupRoleNames;

        public CreateInvitationCommandValidator(IUserRepository userRepository, IGroupRoleRepository groupRoleRepository
            , IInvitationRepository invitationRepository, IUserContextService userContextService
            , IAssignmentRepository assignmentRepository, IGroupRepository groupRepository)
        {
            _userRepository = userRepository;
            _groupRoleRepository = groupRoleRepository;
            _invitationRepository = invitationRepository;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _groupRepository = groupRepository;

            CascadeMode = CascadeMode.Stop;

            RuleFor(r => r.RoleName)
                .NotEmpty().WithMessage("Role hasn't been specified");

            RuleFor(r => r.ReceiverEmail)
                .NotEmpty().WithMessage("Receiver email hasn't been specified")
                .MustAsync(checkIfReceiverExists).WithMessage("Specified receiver doesn't exist");

            RuleFor(g => g.GroupId)
                .NotEmpty().WithMessage("The group hasn't been specified");


            RuleFor(g => g.GroupId)
                .MustAsync
                (async (model, groupId, cancellation) =>
                {
                    return await checkIfInvitationAlreadyExists(model.ReceiverEmail, model.RoleName, groupId, cancellation);
                }).WithMessage("Similar invitation already exists").MustAsync
                (async (model, groupId, cancellation) =>
                {
                    return await checkIfUserIsAlreadyInGroup(model.ReceiverEmail, model.RoleName, groupId, cancellation);
                }).WithMessage("User already is in the group")
                .MustAsync(checkIfSenderIsInTheGroup).WithMessage("You are not in the group");


        }

        private async Task<bool> checkIfInvitationAlreadyExists(string email, string role, int groupId, CancellationToken cancellationToken)
        {
            var receiver = await _userRepository.GetByEmail(email);
            var groupRole = await _groupRoleRepository.GetByName(role);

            bool result = await _invitationRepository
                .AnyAsync(x => x.ReceiverId == receiver.Id && x.Status == InvitationStatusEnum.Sent
                && x.GroupId == groupId && x.GroupRoleId == groupRole.Id);

            if (!result) { return true; }
            return false;
        }

        private async Task<bool> checkIfUserIsAlreadyInGroup(string email, string role, int groupId, CancellationToken cancellationToken)
        {
            var receiver = await _userRepository.GetByEmail(email);
            bool result = await _assignmentRepository.AnyAsync(x => x.UserId == receiver.Id && x.GroupId == groupId);

            if (!result) { return true; }
            return false;
        }

        private async Task<bool> checkIfSenderIsInTheGroup(int groupId, CancellationToken cancellationToken)
        {
            int senderId = (int)_userContextService.GetUserId;
            var result = await _assignmentRepository.AnyAsync(x => x.GroupId == groupId && x.UserId == senderId);

            if (result) { return true; }
            return false;
        }

        private async Task<bool> checkIfReceiverExists(string receiverEmail, CancellationToken cancellationToken)
        {
            return await _userRepository.AnyAsync(x => x.Email == receiverEmail);
        }
    }
}
