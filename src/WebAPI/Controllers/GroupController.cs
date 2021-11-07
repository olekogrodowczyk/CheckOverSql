﻿using Application.Interfaces;
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
            return Ok(new Result<int>(result, "Pomyślnie dodano grupę"));
        }

        
        [HttpGet("getusergroups")]
        public async Task<IActionResult> GetUserGroups()
        {
            var result = await _groupService.GetUserGroups();
            return Ok(new Result<IEnumerable<GetGroupVm>>(result, "Pomyślnie zwrócono wszystkie grupy użytkownika"));
        }
    }
}
