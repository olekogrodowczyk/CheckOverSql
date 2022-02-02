using Application.Dto.AssignExerciseToUsersTo;
using Application.Dto.CreateExerciseDto;
using Application.Exercises.Commands.CreateExercise;
using Application.Exercises.Queries;
using Application.Exercises.Queries.GetAllCreated;
using Application.Exercises.Queries.GetAllPublicExercises;
using Application.Interfaces;
using Application.Responses;
using Application.Groups;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Models;
using Domain.Common;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExerciseController : ApiControllerBase
    {
        private readonly IUserContextService _userContextService;

        public ExerciseController(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> Create([FromBody] CreateExerciseCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "New exercises created succesfully"));
        }

        [HttpGet("GetAllCreated")]
        [ProducesResponseType(200, Type = typeof(Result<PaginatedList<GetExerciseDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllCreatedByLoggedUser([FromQuery] GetAllCreatedExercisesQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<PaginatedList<GetExerciseDto>>
                (result,"All exercises created by logged user returned successfully"));
        }

        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(Result<PaginatedList<GetExerciseDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpGet("GetAllPublic")]
        public async Task<IActionResult> GetAllPublic([FromQuery] GetAllPublicExercisesQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<PaginatedList<GetExerciseDto>>(result, "All public exercises returned successfully"));
        }

        [HttpPost("assignexercise/{id}")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<int>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> AssignExerciseToUsersInGroup
            ([FromRoute] int id, [FromBody] AssignExerciseToUsersCommand command)
        {
            if(id != command.ExerciseId) { return BadRequest(); }
            var result = await Mediator.Send(command);   
            return Ok(new Result<IEnumerable<int>>(result, "Created solving identifiers returned successfully"));
        }
    }
}
