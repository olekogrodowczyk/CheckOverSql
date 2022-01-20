using Application.Databases.Commands.SendQueryAdmin;
using Application.Databases.Queries.GetDatabaseNames;
using Application.Databases.Queries.GetQueryValueAdmin;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        [ProducesResponseType(200, Type = typeof(Result<int>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpPost]
        public async Task<IActionResult> SendQueryAdmin(SendQueryAdminCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Query executed successfully, number of rows affected returned."));
        }

        [Authorize(Roles ="Admin")]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<IEnumerable<string>>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpPost("SendQueryValueAdmin")]
        public async Task<IActionResult> GetQueryValueAdmin([FromBody] GetQueryValueAdminQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<IEnumerable<IEnumerable<string>>>(result, "Query executed successfully, data returned"));
        }

        [Authorize]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<string>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpGet("getdatabasenames")]
        public async Task<IActionResult> GetDatabaseNames()
        {
            var result = await Mediator.Send(new GetDatabaseNamesQuery());
            return Ok(new Result<IEnumerable<string>>(result, "Database names returned successfully"));
        }
    }
}
