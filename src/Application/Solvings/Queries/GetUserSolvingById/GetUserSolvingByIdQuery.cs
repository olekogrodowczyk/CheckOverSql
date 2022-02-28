using Application.Authorization;
using Application.Common.Authorization;
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
        private readonly ISolutionService _solutionService;

        public GetUserSolvingByIdQueryHandler(ISolvingRepository solvingRepository, IAuthorizationService authorizationService
            ,IUserContextService userContextService, IAssignmentRepository assignmentRepository, IMapper mapper
            ,ISolutionService solutionService)
        {
            _solvingRepository = solvingRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _solutionService = solutionService;
        }

        public async Task<GetSolvingDto> Handle(GetUserSolvingByIdQuery request, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;
            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }
            var solving = await _solvingRepository.GetSolvingWithIncludes(request.SolvingId);
            await checkAuthorizations(loggedUserId, solving);
            var solvingDto = _mapper.Map<GetSolvingDto>(solving);
            if (solving.Exercise is not null)
            {
                solvingDto.Exercise.LastAnswer = await _solutionService.GetLatestSolutionQuerySentIntoExercise(solvingDto.Exercise.Id);
            }
            return solvingDto;
        }

        private async Task checkAuthorizations(int? loggedUserId, Solving solving)
        {
            var authorizationGetSolvingByIdResult = await _authorizationService.AuthorizeAsync
                            (_userContextService.UserClaimPrincipal, solving, new GetSolvingByIdRequirement());
            if (!authorizationGetSolvingByIdResult.Succeeded)
            {
                var loggedUserAssignment =
                await _assignmentRepository.GetUserAssignmentBasedOnOtherAssignment((int)loggedUserId, (int)solving.AssignmentId);
                var authorizationPermissionRequirement = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, loggedUserAssignment, new PermissionRequirement(PermissionNames.CheckingExercises));
            }
        }
    }
}
