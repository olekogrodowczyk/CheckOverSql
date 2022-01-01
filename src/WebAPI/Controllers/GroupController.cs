using Application.Interfaces;
using Application.Dto.CreateGroupVm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Responses;
using System.Collections;
using System.Collections.Generic;
using Application.Groups;
using Application.Groups.Commands.CreateGroup;
using Application.Groups.Queries.GetUserGroups;
using Application.Groups.Queries;
using Application.Groups.Commands.DeleteGroup;
using Application.Groups.Queries.GetAllAssignmentsInGroup;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ApiControllerBase
    {

        public GroupController()
        {
        }
        
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> Create([FromBody] CreateGroupCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Group created successfully"));
        }

        [HttpGet("getusergroups")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetGroupDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetUserGroups()
        {
            var result = await Mediator.Send(new GetUserGroupsQuery());
            return Ok(new Result<IEnumerable<GetGroupDto>>(result, "Groups created by user returned successfully"));
        }

        [HttpDelete("deletegroup/{groupId}")]
        [ProducesResponseType(200, Type = typeof(Result))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            await Mediator.Send(new DeleteGroupCommand { GroupId = groupId });
            return Ok(new Result("Group deleted successfully"));
        }

        [HttpGet("getallassignments/{groupId}")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetAssignmentDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllAssignmentsInGroup([FromRoute] int groupId)
        {
            var result = await Mediator.Send(new GetAllAssignmentsInGroupQuery { GroupId = groupId });
            return Ok(new Result<IEnumerable<GetAssignmentDto>>(result, "All assignments returned successfully"));
        }
    }
}
