using Application.Interfaces;
using Application.Dto.CreateGroupVm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Responses;
using System.Collections;
using System.Collections.Generic;
using Application.ViewModels;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost] 
        public async Task<IActionResult> Create([FromBody] CreateGroupDto model)
        {
            var result = await _groupService.CreateGroup(model);
            return Ok(new Result<int>(result, "Group created successfully"));
        }

        
        [HttpGet("getusergroups")]
        public async Task<IActionResult> GetUserGroups()
        {
            var result = await _groupService.GetUserGroups();
            return Ok(new Result<IEnumerable<GetGroupVm>>(result, "Groups created by user returned successfully"));
        }

        [HttpDelete("deletegroup/{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            await _groupService.DeleteGroup(groupId);
            return Ok(new Result("Group deleted successfully"));
        }

        [HttpGet("getallassignments/{groupId}")]
        public async Task<IActionResult> GetAllAssignmentsInGroup([FromRoute] int groupId)
        {
            var result = await _groupService.GetAllAssignmentsInGroup(groupId);
            return Ok(new Result<IEnumerable<GetAssignmentVm>>(result, "All assignments returned successfully"));
        }
    }
}
