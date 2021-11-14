using Application.Interfaces;
using Application.Responses;
using Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SolvingController : ControllerBase
    {
        private readonly ISolvingService _solvingService;

        public SolvingController(ISolvingService solvingService)
        {
            _solvingService = solvingService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAllSolvingAssignedToUser()
        {
            await _solvingService.GetAllSolvingsAssignedToUser();
            return Ok(new Result("Solution assigned to solving successfully"));
        }

        [HttpGet("getbyid/{solvingId}")]
        public async Task<IActionResult> GetUserSolvingById([FromRoute] int solvingId)
        {
            var result = await _solvingService.GetSolvingById(solvingId);
            return Ok(new Result<GetSolvingVm>(result, "Solving returned successfully"));
        }
    }
}
