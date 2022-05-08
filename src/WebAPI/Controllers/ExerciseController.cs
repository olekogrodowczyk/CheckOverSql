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
using Application.Exercises.Queries.CheckIfUserCanAssigneExercise;
using Application.Exercises.Queries.GetExerciseById;

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

        [HttpGet("GetById/{exerciseId}")]
        [ProducesResponseType(200, Type = typeof(Result<GetExerciseDto>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetById([FromRoute] int exerciseId)
        {
            var result = await Mediator.Send(new GetExerciseByIdQuery { ExerciseId = exerciseId });
            return Ok(new Result<GetExerciseDto>(result, "Exercises returned successfully"));
        }

        [HttpPost("CreateExercise")]
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
                (result, "All exercises created by logged user returned successfully"));
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

        [HttpPost("AssignExercise")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<int>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> AssignExerciseToUsersInGroup
            ([FromBody] AssignExerciseToUsersCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<IEnumerable<int>>(result, "Created solving identifiers returned successfully"));
        }

        [HttpGet("CheckIfUserCanAssignExercise")]
        [ProducesResponseType(200, Type = typeof(Result<bool>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> CheckIfUserCanAssignExercise()
        {
            var result = await Mediator.Send(new CheckIfUserCanAssignExerciseQuery());
            return Ok(new Result<bool>(result, "Query result has been returned successfully"));
        }
    }
}
