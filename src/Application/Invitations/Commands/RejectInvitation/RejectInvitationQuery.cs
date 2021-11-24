using Application.Common.Exceptions;
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
    public class RejectInvitationQuery : IRequest
    {
        public int InvitationId { get; set; }
    }

    public class RejectInvitationQueryHandler : IRequestHandler<RejectInvitationQuery>
    {
        private readonly IInvitationRepository _invitationRepository;

        public RejectInvitationQueryHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task<Unit> Handle(RejectInvitationQuery request, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId);
            if (invitation.Status != InvitationStatusEnum.Sent.ToString())
            {
                throw new BadRequestException("The invitation isn't pending", true);
            }

            invitation.Status = InvitationStatusEnum.Rejected.ToString();
            await _invitationRepository.UpdateAsync(invitation);
            return Unit.Value;
        }
    }
}
