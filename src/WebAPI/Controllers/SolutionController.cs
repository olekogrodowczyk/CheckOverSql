using Application.Dto.CreateSolutionDto;
using Application.Dto.SendQueryDto;
using Application.Interfaces;
using Application.Responses;
using Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/exercise/{exerciseId}/[controller]")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private readonly ISolutionService _solutionService;

        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromRoute] int exerciseId)
        {
            var result = await _solutionService.GetAllSolutions(exerciseId);
            return Ok(new Result<IEnumerable<GetSolutionVm>>(result, "All solution returned successfully"));
        }
        
        [HttpPost()]
        public async Task<IActionResult> Create([FromRoute] int exerciseId, [FromBody] CreateSolutionDto model)
        {
            var result = await _solutionService.CreateSolution(exerciseId, model);
            return Ok(new Result<GetComparisonVm>(result, "New solution added successfully and returned comparison"));
        }

        [HttpGet("getquerydata/{solutionId}")]
        public async Task<IActionResult> GetQueryData([FromRoute] int exerciseId, [FromRoute] int solutionId)
        {
            var result = await _solutionService.SendSolutionQuery(solutionId);
            return Ok(new Result<List<List<string>>>(result, "Query executed successfully"));
        }

            


    }
}
