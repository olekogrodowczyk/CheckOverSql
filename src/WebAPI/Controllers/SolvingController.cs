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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolvingController : ApiControllerBase
    {
        private readonly IUserContextService _userContextService;

        public SolvingController(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllSolvingsAssignedToUser()
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var result = await Mediator.Send(new GetAllSolvingsAssignedToUserQuery { UserId = loggedUserId });
            return Ok(new Result<IEnumerable<GetSolvingDto>>(result, "All solvings returned successfully"));
        }

        [HttpGet("getbyid/{solvingId}")]
        public async Task<IActionResult> GetUserSolvingById([FromRoute] int solvingId)
        {
            var result = await Mediator.Send(new GetUserSolvingByIdQuery { SolvingId = solvingId });
            return Ok(new Result<GetSolvingDto>(result, "Solving returned successfully"));
        }

        [HttpGet("getalltodo")]
        public async Task<IActionResult> GetAllSolvingsAssignedToUserToDo()
        {
            int loggedUserId = (int)_userContextService.GetUserId;
            var result = await Mediator.Send(new GetAllSolvingsAssignedToUserToDoQuery { UserId = loggedUserId });
            return Ok(new Result<IEnumerable<GetSolvingDto>>(result, "All solvings to do returned successfully"));
        }
    }
}
