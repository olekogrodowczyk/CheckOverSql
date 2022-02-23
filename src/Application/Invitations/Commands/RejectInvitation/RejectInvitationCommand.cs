using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invitations.Commands.RejectInvitation
{
    public class RejectInvitationCommand : IRequest
    {
        public int InvitationId { get; set; }
    }

    public class RejectInvitationQueryHandler : IRequestHandler<RejectInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUserContextService _userContextService;

        public RejectInvitationQueryHandler(IInvitationRepository invitationRepository, IUserContextService userContextService)
        {
            _invitationRepository = invitationRepository;
            _userContextService = userContextService;
        }

        public async Task<Unit> Handle(RejectInvitationCommand request, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId);
            if (invitation.Status != InvitationStatusEnum.Sent.ToString())
            {
                throw new BadRequestException("The invitation isn't pending", true);
            }
            checkForLoggedUser(invitation);

            invitation.Status = InvitationStatusEnum.Rejected.ToString();
            await _invitationRepository.UpdateAsync(invitation);
            return Unit.Value;
        }

        private void checkForLoggedUser(Invitation invitation)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            if (invitation.ReceiverId != (int)loggedUserId)
            {
                throw new BadRequestException("You cannot reject this invitation", true);
            }
        }
    }
}
