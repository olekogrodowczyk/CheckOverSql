using Application.GroupRoles.Queries;
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

    }
}
