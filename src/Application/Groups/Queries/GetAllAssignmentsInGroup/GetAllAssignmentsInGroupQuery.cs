using Application.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries.GetAllAssignmentsInGroup
{
    public class GetAllAssignmentsInGroupQuery : IRequest<IEnumerable<GetAssignmentDto>>
    {
        public int GroupId { get; set; }
    }

    public class GetAllAssignmentsInGroupQueryHandler : IRequestHandler<GetAllAssignmentsInGroupQuery, IEnumerable<GetAssignmentDto>>
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IGroupRepository _groupRepository;
        private readonly IMapper _mapper;

        public GetAllAssignmentsInGroupQueryHandler(IAssignmentRepository assignmentRepository, IUserContextService userContextService
            ,IAuthorizationService authorizationService, IGroupRepository groupRepository, IMapper mapper)
        {
            _assignmentRepository = assignmentRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
            _groupRepository = groupRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAssignmentDto>> Handle(GetAllAssignmentsInGroupQuery request, CancellationToken cancellationToken)
        {
            Assignment userAssignment = await _assignmentRepository
                .SingleOrDefaultAsync(x => x.UserId == _userContextService.GetUserId && x.GroupId == request.GroupId);
            if (userAssignment == null) { throw new ForbidException("You're not in the group", true); }

            await _authorizationService.AuthorizeAsync(_userContextService.UserClaimPrincipal
                , userAssignment, new PermissionRequirement(PermissionNames.GettingAssignments));

            var assignments = await _assignmentRepository.GetWhereAsync(x => x.GroupId == request.GroupId, x => x.User);
            var assignmentDtos = _mapper.Map<IEnumerable<GetAssignmentDto>>(assignments);
            return assignmentDtos;
        }
    }
}
