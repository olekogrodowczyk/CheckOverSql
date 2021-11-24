using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Exercises.Commands.CreateExercise;
using Application.Exercises.Queries;
using Application.Exercises.Queries.GetAllCreated;
using Application.Exercises.Queries.GetAllPublicExercises;
using Application.Interfaces;
using Application.Responses;
using Application.Solvings;
using MediatR;
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
    public class ExerciseController : ApiControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly IUserContextService _userContextService;

        public ExerciseController(IExerciseService exerciseService, IUserContextService userContextService)
        {
            _exerciseService = exerciseService;
            _userContextService = userContextService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateExerciseCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "New exercises created succesfully"));
        }

        [HttpGet("getallcreated")]
        public async Task<IActionResult> GetAllCreatedByLoggedUser()
        {
            var result = await Mediator.Send(new GetAllCreatedExercisesQuery());
            return Ok(new Result<IEnumerable<GetExerciseDto>>
                (result,"All exercises created by logged user returned successfully"));
        }

        [HttpGet("getallpublic")]
        public async Task<IActionResult> GetAllPublic()
        {
            var result = await Mediator.Send(new GetAllPublicExercisesQuery());
            return Ok(new Result<IEnumerable<GetExerciseDto>>(result, "All public exercises returned successfully"));
        }

        [HttpPost("assignexercise/{id}")]
        public async Task<IActionResult> AssignExerciseToUsersInGroup
            ([FromRoute] int id, [FromBody] AssignExerciseToUsersCommand command)
        {
            if(id != command.ExerciseId) { return BadRequest(); }
            await _exerciseService.CheckIfUserCanAssignExerciseToUsers(command.GroupId);
            var result = await Mediator.Send(command);   
            return Ok(new Result<IEnumerable<int>>(result, "Created solving identifiers returned successfully"));
        }
    }
}
