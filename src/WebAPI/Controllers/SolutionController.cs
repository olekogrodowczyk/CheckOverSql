using Application.Dto.CreateSolutionDto;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/exercise/{exerciseId}/solution")]
    [ApiController]
    public class SolutionController : ControllerBase
    {
        private readonly ISolutionService _solutionService;

        public SolutionController(ISolutionService solutionService)
        {
            _solutionService = solutionService;
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromRoute] int exerciseId, [FromBody] CreateSolutionDto model)
        {
            var result =  await _solutionService.CreateSolutionAsync(model, exerciseId);
            return Ok(new Result<int>(result, "Pomyślnie dodano nowe rozwiązanie"));
        }

        [HttpGet("getquerydata/{solutionId}")]
        public async Task<IActionResult> GetQueryData([FromRoute] int solutionId)
        {
            var result = await _solutionService.SendSolutionQuery(solutionId);
            return Ok(new Result<Dictionary<int,object>>(result, "Pomyślnie zwrócono wyniki zapytania do bazy"));
        }
    }
}
