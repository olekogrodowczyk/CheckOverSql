using Application.Authorization;
using Application.Common.Authorization;
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
        private readonly IGroupRepository _groupRepository;

        public GetUserSolvingByIdQueryHandler(ISolvingRepository solvingRepository, IAuthorizationService authorizationService
            , IUserContextService userContextService, IAssignmentRepository assignmentRepository, IMapper mapper
            , ISolutionService solutionService, IGroupRepository groupRepository)
        {
            _solvingRepository = solvingRepository;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
            _solutionService = solutionService;
            _groupRepository = groupRepository;
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
                solvingDto.Exercise.Passed = await _solutionService.CheckIfUserPassedExercise(solvingDto.Exercise.Id);
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
                int groupId = (await _solvingRepository
                .SingleOrDefaultAsync(x => x.Id == solving.Id, x => x.Assignment)).Assignment.GroupId;
                var group = await _groupRepository.GetByIdAsync(groupId);
                var authorizationPermissionRequirementResult = await _authorizationService.AuthorizeAsync
                (_userContextService.UserClaimPrincipal, group, new PermissionRequirement(PermissionEnum.CheckingExercises));
                if (!authorizationPermissionRequirementResult.Succeeded) { throw new ForbidException(PermissionEnum.CheckingExercises); }
            }
        }
    }
}
