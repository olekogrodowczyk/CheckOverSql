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

namespace Application.Invitations.Commands.AcceptInvitation
{
    public class AcceptInvitationQuery : IRequest
    {
        public int InvitationId { get; set; }
    }

    public class AcceptInvitationQueryHandler : IRequestHandler<AcceptInvitationQuery>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;

        public AcceptInvitationQueryHandler(IInvitationRepository invitationRepository, IAssignmentRepository assignmentRepository
            ,IUserContextService userContextService)
        {
            _invitationRepository = invitationRepository;
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
        }

        public async Task<Unit> Handle(AcceptInvitationQuery request, CancellationToken cancellationToken)
        {
            var invitation = await _invitationRepository.GetByIdAsync(request.InvitationId);
            if (invitation.Status != InvitationStatusEnum.Sent.ToString())
            {
                throw new BadRequestException("The invitation isn't pending", true);
            }
            checkForLoggedUser(invitation);

            await _assignmentRepository.AddAsync(new Assignment
            {
                GroupId = (int)invitation.GroupId,
                GroupRoleId = invitation.GroupRoleId,
                UserId = invitation.ReceiverId,
            });

            invitation.Status = InvitationStatusEnum.Accepted.ToString();
            await _invitationRepository.UpdateAsync(invitation);
            return Unit.Value;
        }

        private void checkForLoggedUser(Invitation invitation)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            if (invitation.ReceiverId != (int)loggedUserId)
            {
                throw new BadRequestException("You cannot accept this invitation", true);
            }
        }
    }
}
