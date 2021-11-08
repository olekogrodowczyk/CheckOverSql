using Application.Dto.CreateInvitationDto;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            await _invitationService.CheckIfInvitationAlreadyExists(model.ReceiverEmail, model.RoleName, groupId);
            await _invitationService.CheckIfUserIsAlreadyInGroup(model.ReceiverEmail, model.RoleName, groupId);
            int result = await _invitationService.CreateInvitation(model, groupId);
            return Ok(new Result<int>(result, "Invitation sent successfully"));
        }
    }
}
