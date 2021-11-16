using Application.Dto.SendQueryDto;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatabaseController : ControllerBase
    {
        private readonly IDatabaseService _queryService;

        public DatabaseController(IDatabaseService queryService)
        {
            _queryService = queryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendQueryAdmin(SendQueryDto model)
        {
            var result = await _queryService.SendQueryNoData(model.Query, model.Database, true);
            return Ok(new Result<int>(result, "Pomyślnie wykonano zapytanie. Zwrócono ilość zmienionych rzędów."));
        }
    }
}
