using Application.GroupRoles.Commands;
using Application.GroupRoles.Queries;
using Application.GroupRoles.Queries.CheckPermission;
using Application.GroupRoles.Queries.GetAllGroupRoles;
using Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupRoleController : ApiControllerBase
    {
        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GroupRoleDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllGroupRoles()
        {
            var result = await Mediator.Send(new GetAllGroupRolesQuery());
            return Ok(new Result<IEnumerable<GroupRoleDto>>(result, "All group roles returned successfully"));
        }

        [HttpGet("CheckPermission")]
        [ProducesResponseType(200, Type = typeof(Result<bool>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetGroupRoles([FromQuery] CheckPermissionQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<bool>(result, "Result returned successfully"));
        }

        [HttpPatch("ChangeRole")]
        [ProducesResponseType(200, Type = typeof(Result<>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> ChangeRole([FromQuery] ChangeRoleCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "The resolt has been changed successfully"));
        }

    }
}
