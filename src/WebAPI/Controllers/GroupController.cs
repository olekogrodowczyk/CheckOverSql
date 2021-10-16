using Application.Interfaces;
using Application.Dto.CreateGroupVm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Responses;
using System.Collections;
using Application.Dto.GetGroupDto;
using System.Collections.Generic;

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

        [HttpPost("creategroup")] 
        public async Task<IActionResult> Create([FromBody] CreateGroupDto model)
        {
            var result = await _groupService.CreateGroupAsync(model);
            return Ok(new Result<int>(result, "Pomyślnie dodano grupę"));
        }

        
        [HttpGet("getusergroups")]
        public async Task<IActionResult> GetUserGroups()
        {
            var result = await _groupService.GetUserGroups();
            return Ok(new Result<IEnumerable<GetGroupDto>>(result, "Pomyślnie zwrócono wszystkie grupy użytkownika"));
        }
    }
}
