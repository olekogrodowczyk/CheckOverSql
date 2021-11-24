using Application.Common.Exceptions;
using Application.Groups;
using AutoMapper;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Invitations.Queries.GetAllInvitationReceived
{
    public class GetAllInvitationsQuery : IRequest<IEnumerable<GetInvitationDto>>
    {
        public int UserId { get; set; }
        public string TypeOfGetInvitationsQuery { get; set; }
    }

    public class GetAllInvitationsReceivedQueryHandler : IRequestHandler<GetAllInvitationsQuery, IEnumerable<GetInvitationDto>>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IMapper _mapper;

        public GetAllInvitationsReceivedQueryHandler(IInvitationRepository invitationRepository, IMapper mapper)
        {
            _invitationRepository = invitationRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetInvitationDto>> Handle(GetAllInvitationsQuery command, CancellationToken cancellationToken)
        {
            var invitations = await _invitationRepository.GetInvitationsWithAllIncludes();
            switch(command.TypeOfGetInvitationsQuery.ToLower())
            {
                case "received":
                    invitations = invitations.Where(x => x.ReceiverId == command.UserId);
                    break;
                case "sent":
                    invitations = invitations.Where(x => x.SenderId == command.UserId);
                    break;
                case "all":
                    break;
                default:
                    throw new BadRequestException("TypeOfGetInvitationsQuery is wrong", true);
            }     
            var invitationsVm = _mapper.Map<IEnumerable<GetInvitationDto>>(invitations);
            return invitationsVm;
        }
    }
}
