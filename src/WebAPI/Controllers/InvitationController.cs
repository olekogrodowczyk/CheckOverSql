using Application.Dto.CreateInvitationDto;
using Application.Interfaces;
using Application.Responses;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/group/{groupId}/[controller]")]
    [ApiController]
    public class InvitationController : ControllerBase
    {
        private readonly IInvitationService _invitationService;

        public InvitationController(IInvitationService invitationService)
        {
            _invitationService = invitationService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateInvitationDto model, [FromRoute] int groupId)
        {
            await _invitationService.CheckIfSenderIsInTheGroup(groupId);
            await _invitationService.CheckIfInvitationAlreadyExists(model.ReceiverEmail, model.RoleName, groupId);
            await _invitationService.CheckIfUserIsAlreadyInGroup(model.ReceiverEmail, model.RoleName, groupId);        
            int result = await _invitationService.CreateInvitation(model, groupId);
            return Ok(new Result<int>(result, "Invitation sent successfully"));
        }

        [HttpGet("getallreceived")]
        public async Task<IActionResult> GetAllReceived()
        {
            var result = await _invitationService.GetAllUserReceivedInvitations();
            return Ok(new Result<IEnumerable<GetInvitationVm>>(result, "Received invitations returned successfully"));
        }

        [HttpGet("getallsent")]
        public async Task<IActionResult> GetAllSent()
        {
            var result = await _invitationService.GetAllUserSentInvitations();
            return Ok(new Result<IEnumerable<GetInvitationVm>>(result, "Sent invitations returned successfully"));
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _invitationService.GetAllUserInvitations();
            return Ok(new Result<IEnumerable<GetInvitationVm>>(result, "All Invitations returned successfully"));
        }

        [HttpPatch("accept/{invitationId}")]
        public async Task<IActionResult> Accept([FromRoute] int invitationId)
        {
            await _invitationService.AcceptInvitation(invitationId);
            return Ok(new Result("Invitation accepted successfully"));
        }

        [HttpPatch("reject/{invitationId}")]
        public async Task<IActionResult> Reject([FromRoute] int invitationId)
        {
            await _invitationService.RejectInvitation(invitationId);
            return Ok(new Result("Invitation accepted successfully"));
        }

    }
}
