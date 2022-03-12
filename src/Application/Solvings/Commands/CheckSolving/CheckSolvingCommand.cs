using Application.Authorization;
using Application.Common.Exceptions;
using Application.Interfaces;
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

namespace Application.Solvings.Commands.CheckExercise
{
    public class CheckSolvingCommand : IRequest<int>
    {
        public int SolvingId { get; set; }
        public string Remarks { get; set; }
        public int Points { get; set; }
    }

    public class CheckExerciseCommandHandler : IRequestHandler<CheckSolvingCommand, int>
    {
        private readonly ISolvingRepository _solvingRepository;
        private readonly IRepository<Checking> _checkingRepository;
        private readonly IUserContextService _userContextService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IGroupRepository _groupRepository;

        public CheckExerciseCommandHandler(ISolvingRepository solvingRepository, IRepository<Checking> checkingRepository
            , IUserContextService userContextService, IAuthorizationService authorizationService, IGroupRepository groupRepository)
        {
            _solvingRepository = solvingRepository;
            _checkingRepository = checkingRepository;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
            _groupRepository = groupRepository;
        }

        public async Task<int> Handle(CheckSolvingCommand command, CancellationToken cancellationToken)
        {
            int? loggedUserId = _userContextService.GetUserId;

            if (loggedUserId is null) { throw new UnauthorizedAccessException(); }

            var solving = await _solvingRepository.GetByIdAsync(command.SolvingId);
            if (solving is null) { throw new NotFoundException(nameof(solving), command.SolvingId); }

            int groupId = (await _solvingRepository
                .SingleOrDefaultAsync(x => x.Id == solving.Id, x => x.Assignment)).Assignment.GroupId;


            await handleAuthorization(groupId);

            return await createCheckingAndChangeSolvingStatus(command, loggedUserId, solving);
        }

        private async Task handleAuthorization(int groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            var autorizationResult = await _authorizationService.AuthorizeAsync
                            (_userContextService.UserClaimPrincipal, group, new PermissionRequirement(PermissionEnum.CheckingExercises));
            if (!autorizationResult.Succeeded) { throw new ForbidException(PermissionEnum.CheckingExercises); }
        }

        private async Task<int> createCheckingAndChangeSolvingStatus(CheckSolvingCommand command, int? loggedUserId, Solving solving)
        {
            Checking checking = new Checking()
            {
                SolvingId = command.SolvingId,
                CheckerId = (int)loggedUserId,
                Points = command.Points,
                Remarks = command.Remarks,
            };
            await _checkingRepository.AddAsync(checking);
            solving.Status = SolvingStatusEnum.Checked;
            await _solvingRepository.UpdateAsync(solving);
            return checking.Id;
        }
    }
}

