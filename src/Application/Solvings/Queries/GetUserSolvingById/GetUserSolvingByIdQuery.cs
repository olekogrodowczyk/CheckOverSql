using Application.Authorization;
using Application.Common.Authorization;
using Application.Interfaces;
using AutoMapper;
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

namespace Application.Groups.Queries.GetUserSolvingById
{
    public class GetUserSolvingByIdQuery : IRequest<GetSolvingDto>
    {
        public int SolvingId { get; set; }
    }

    public class GetUserSolvingByIdQueryHandler : IRequestHandler<GetUserSolvingByIdQuery, GetSolvingDto>
    {
        private readonly ISolvingRepository _solvingRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public GetUserSolvingByIdQueryHandler(ISolvingRepository solvingRepository, IAuthorizationService authorizationService
            ,IUserContextService userContextService, IAssignmentRepository assignmentRepository, IMapper mapper)
        {
            _solvingRepository = solvingRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<GetSolvingDto> Handle(GetUserSolvingByIdQuery request, CancellationToken cancellationToken)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var solving = await _solvingRepository.GetSolvingWithIncludes(request.SolvingId);
            var authorizationGetSolvingByIdResult = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, solving, new GetSolvingByIdRequirement());

            if (!authorizationGetSolvingByIdResult.Succeeded)
            {
                //Solving exists and now we should check the permission
                var loggedUserAssignment =
                await _assignmentRepository.GetUserAssignmentBasedOnOtherAssignment(loggedUserId, (int)solving.AssignmentId);

                var authorizationPermissionRequirement = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, loggedUserAssignment, new PermissionRequirement(PermissionNames.CheckingExercises));
            }

            var solvingDto = _mapper.Map<GetSolvingDto>(solving);
            return solvingDto;
        }
    }
}
