using Application.Common.Exceptions;
using Application.Interfaces;
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

namespace Application.Invitations.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandValidator : AbstractValidator<AcceptInvitationCommand>
    {
        private readonly IUserContextService _userContextService;
        private readonly IInvitationRepository _invitationRepository;

        public AcceptInvitationCommandValidator(IUserContextService userContextService, IInvitationRepository invitationRepository)
        {
            _userContextService = userContextService;
            _invitationRepository = invitationRepository;

            RuleFor(x => x.InvitationId)
                .NotEmpty().WithMessage("Invitation id cannot be empty")
                .MustAsync(checkForLoggedUser).WithMessage("You cannot accept this invitation")
                .MustAsync(checkStatus).WithMessage("The invitation isn't pending");
            
        }

        private async Task<bool> checkForLoggedUser(int invitationId, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var invitation = await _invitationRepository.GetByIdAsync(invitationId);
            if(invitation is null) { throw new NotFoundException(nameof(Invitation), invitation.Id); }

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
