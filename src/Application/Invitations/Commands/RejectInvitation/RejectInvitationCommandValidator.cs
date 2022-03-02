using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Invitations.Commands.AcceptInvitation;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invitations.Commands.RejectInvitation
{
    public class RejectInvitationCommandValidator : AbstractValidator<RejectInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;

        public RejectInvitationCommandValidator(IInvitationRepository invitationRepository, IAssignmentRepository assignmentRepository
            , IUserContextService userContextService)
        {
            _invitationRepository = invitationRepository;
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
        }

        private async Task<bool> checkForLoggedUser(int invitationId, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation is null) { throw new NotFoundException(nameof(Invitation), invitation.Id); }

            if (invitation.ReceiverId != (int)loggedUserId) { return false; }
            return true;

        }

        private async Task<bool> checkStatus(int invitationId, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if (invitation is null) { throw new NotFoundException(nameof(Invitation), invitation.Id); }

            if (invitation.Status != InvitationStatusEnum.Sent) { return false; }
            return true;
        }
    }
}
