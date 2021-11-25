using Application.Dto.CreateInvitationDto;
using Application.Interfaces;
using Application.Invitations.Commands.AcceptInvitation;
using Application.Invitations.Commands.CreateInvitation;
using Application.Invitations.Commands.RejectInvitation;
using Application.Invitations.Queries.GetAllInvitationReceived;
using Application.Responses;
using Application.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationController : ApiControllerBase
    {
        private readonly IInvitationService _invitationService;
        private readonly IUserContextService _userContextService;

        public InvitationController(IInvitationService invitationService, IUserContextService userContextService)
        {
            _invitationService = invitationService;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvitationCommand command)
        {
            await _invitationService.CheckIfSenderIsInTheGroup(command.GroupId);
            await _invitationService.CheckIfInvitationAlreadyExists(command.ReceiverEmail, command.RoleName, command.GroupId);
            await _invitationService.CheckIfUserIsAlreadyInGroup(command.ReceiverEmail, command.RoleName, command.GroupId);
            int result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Invitation sent successfully"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] string queryType)
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var command = new GetAllInvitationsQuery { QueryType = queryType, UserId = loggedUserId };
            var result = await Mediator.Send(command);
            return Ok(new Result<IEnumerable<GetInvitationDto>>(result, "All Invitations returned successfully"));
        }

        [HttpPatch("accept/{invitationId}")]
        public async Task<IActionResult> Accept([FromRoute] int invitationId)
        {
            await Mediator.Send(new AcceptInvitationQuery { InvitationId = invitationId });
            return Ok(new Result("Invitation accepted successfully"));
        }

        [HttpPatch("reject/{invitationId}")]
        public async Task<IActionResult> Reject([FromRoute] int invitationId)
        {
            await Mediator.Send(new RejectInvitationQuery { InvitationId = invitationId });
            return Ok(new Result("Invitation accepted successfully"));
        }

    }
}
