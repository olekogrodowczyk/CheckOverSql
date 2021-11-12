using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Exceptions;
using Application.Interfaces;
using Application.Responses;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseDto model)
        {
            var result = await _exerciseService.CreateExercise(model);
            return Ok(new Result<int>(result, "New exercises created succesfully"));
        }

        [HttpGet("getallcreated")]
        public async Task<IActionResult> GetAllCreated()
        {
            var result = await _exerciseService.GetAllExercisesCreatedByLoggedUser();
            return Ok(new Result<IEnumerable<GetExerciseVm>>
                (result,"All exercises created by logged user returned successfully"));
        }

        [HttpGet("getallpublic")]
        public async Task<IActionResult> GetAllPublic()
        {
            var result = await _exerciseService.GetAllPublicExercises();
            return Ok(new Result<IEnumerable<GetExerciseVm>>(result, "All public exercises returned successfully"));
        }

        [HttpPost("assignexercise")]
        public async Task<IActionResult> AssignExerciseToUsersInGroup
            ([FromQuery] int groupId, [FromQuery] int exerciseId, [FromBody] AssignExerciseToUsersDto model)
        {
            await _exerciseService.CheckIfUserCanAssignExerciseToUsers(groupId);
            var result = await _exerciseService.AssignExerciseToAllUsers(groupId, exerciseId, model);
            return Ok(new Result<IEnumerable<int>>(result, "Created solving identifiers returned successfully"));
        }
    }
}
