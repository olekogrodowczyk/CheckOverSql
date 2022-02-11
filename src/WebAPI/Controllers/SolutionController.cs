using Application.Interfaces;
using Application.Responses;
using Application.Solutions;
using Application.Solutions.Commands.CreateSolution;
using Application.Solutions.Commands.SendSolutionQuery;
using Application.Solutions.Queries;
using Application.Solutions.Queries.GetAllSolutionsCreatedByUser;
using Application.Groups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Solutions.Queries.GetLastQueryInExercise;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SolutionController : ApiControllerBase
    {
        private readonly ISolutionService _solutionService;
        private readonly IUserContextService _userContextService;

        public SolutionController(ISolutionService solutionService, IUserContextService userContextService)
        {
            _solutionService = solutionService;
            _userContextService = userContextService;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<GetSolutionDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetAllCreatedByUser()
        {
            int userId = (int)_userContextService.GetUserId;
            var result = await Mediator.Send(new GetAllSolutionsCreatedByUserQuery { UserId = userId });
            return Ok(new Result<IEnumerable<GetSolutionDto>>(result, "All solution returned successfully"));
        }
        
        [HttpPost("CreateSolution")]
        [ProducesResponseType(200, Type = typeof(Result<GetComparisonDto>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> Create([FromBody] CreateSolutionCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<GetComparisonDto>(result, "New solution added successfully and returned comparison"));
        }

        [HttpGet("GetQueryData/{solutionId}")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<IEnumerable<string>>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetQueryData([FromQuery] int exerciseId, [FromRoute] int solutionId)
        {
            var command = new SendSolutionQueryCommand { ExerciseId = exerciseId, SolutionId = solutionId };
            var result = await Mediator.Send(command);
            return Ok(new Result<IEnumerable<IEnumerable<string>>>(result, "Query executed successfully"));
        }

        [HttpGet("GetLastSolutionSentIntoExercise")]
        [ProducesResponseType(200, Type = typeof(Result<GetSolutionDto>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        public async Task<IActionResult> GetLastSolutionSentIntoExercise([FromQuery] GetLastSolutionSentIntoExerciseQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<GetSolutionDto>(result, "Solution returned successfully"));
        }




    }
}
