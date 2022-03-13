using Application.Interfaces;
using Application.Responses;
using Application.Groups;
using Application.Groups.Queries;
using Application.Groups.Queries.GetAllSolvingsAssignedToUser;
using Application.Groups.Queries.GetAllSolvingsAssignedToUserToDo;
using Application.Groups.Queries.GetUserSolvingById;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Application.Solvings.Queries.GetAllSolvingsToCheck;
using Application.Solvings.Commands.CheckExercise;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SolvingController : ApiControllerBase
    {
        private readonly IUserContextService _userContextService;

        public SolvingController()
        {
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetSolvingDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllSolvingsAssignedToUser()
        {
            var result = await Mediator.Send(new GetAllSolvingsAssignedToUserQuery());
            return Ok(new Result<IEnumerable<GetSolvingDto>>(result, "All solvings returned successfully"));
        }

        [HttpGet("GetById/{solvingId}")]
        [ProducesResponseType(200, Type = typeof(Result<GetSolvingDto>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetUserSolvingById([FromRoute] int solvingId)
        {
            var result = await Mediator.Send(new GetUserSolvingByIdQuery { SolvingId = solvingId });
            return Ok(new Result<GetSolvingDto>(result, "Solving returned successfully"));
        }

        [HttpGet("GetAllByStatus")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetSolvingDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllSolvingsAssignedToUserToDo([FromQuery] GetAllSolvingsAssignedToUserByStatusQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<IEnumerable<GetSolvingDto>>(result, "All solvings to do returned successfully"));
        }

        [HttpGet("GetAllToCheck")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetSolvingDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllSolvingsToCheck([FromQuery] GetAllSolvingsToCheckQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<IEnumerable<GetSolvingDto>>(result, "All solvings to check returned successfully"));
        }

        [HttpPost("CheckSolving")]
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> CheckSolving([FromBody] CheckSolvingCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Solving checked successfully"));
        }
    }
}
