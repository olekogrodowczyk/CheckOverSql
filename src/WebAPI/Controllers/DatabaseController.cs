using Application.Databases.Commands.SendQueryAdmin;
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
    public class DatabaseController : ApiControllerBase
    {
        private readonly IDatabaseService _queryService;

        public DatabaseController(IDatabaseService queryService)
        {
            _queryService = queryService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SendQueryAdmin(SendQueryAdminCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Query executed successfully, number of rows affected returned."));
        }
    }
}
