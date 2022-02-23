using Application.Common.Exceptions;
using Application.Groups;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
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
        public string QueryType { get; set; }
    }

    public class GetAllInvitationsReceivedQueryHandler : IRequestHandler<GetAllInvitationsQuery, IEnumerable<GetInvitationDto>>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;

        public GetAllInvitationsReceivedQueryHandler(IInvitationRepository invitationRepository, IMapper mapper
            ,IUserContextService userContextService)
        {
            _invitationRepository = invitationRepository;
            _mapper = mapper;
            _userContextService = userContextService;
        }

        public async Task<IEnumerable<GetInvitationDto>> Handle(GetAllInvitationsQuery command, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if(loggedUserId is null) { throw new UnauthorizedAccessException(); }
            IEnumerable<Invitation> invitations;
            switch(command.QueryType.ToLower())
            {
                case "received":
                    invitations = await _invitationRepository.GetWhereAsync(x => x.ReceiverId == (int)loggedUserId, x=>x.GroupRole);
                    break;
                case "sent":
                    invitations = await _invitationRepository.GetWhereAsync(x => x.SenderId == (int)loggedUserId, x=>x.GroupRole);
                    break;
                case "all":
                    invitations = await _invitationRepository
                        .GetWhereAsync(x => x.SenderId == (int)loggedUserId || x.ReceiverId == (int)loggedUserId, x=>x.GroupRole);
                    break;
                default:
                    throw new BadRequestException("TypeOfGetInvitationsQuery is wrong", true);
            }     
            var invitationsVm = _mapper.Map<IEnumerable<GetInvitationDto>>(invitations);
            return invitationsVm;
        }
    }
}
