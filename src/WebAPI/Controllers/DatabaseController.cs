using Application.Databases.Commands.SendQueryAdmin;
using Application.Databases.Queries;
using Application.Databases.Queries.GetDatabaseNames;
using Application.Databases.Queries.GetQueryHistory;
using Application.Databases.Queries.GetQueryValueAdmin;
using Application.Interfaces;
using Application.Responses;
using Domain.Common;
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
        [HttpPost("SendQueryAdmin")]
        public async Task<IActionResult> SendQueryAdmin(SendQueryAdminCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(new Result<int>(result, "Query executed successfully, number of rows affected returned."));
        }

        [Authorize]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<IEnumerable<string>>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpPost("GetQueryValue")]
        public async Task<IActionResult> GetQueryValue([FromBody] GetQueryValueQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<IEnumerable<IEnumerable<string>>>(result, "Query executed successfully, data returned"));
        }

        [Authorize]
        [ProducesResponseType(200, Type = typeof(Result<IEnumerable<string>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpGet("GetDatabaseNames")]
        public async Task<IActionResult> GetDatabaseNames()
        {
            var result = await Mediator.Send(new GetDatabaseNamesQuery());
            return Ok(new Result<IEnumerable<string>>(result, "Database names returned successfully"));
        }

        [Authorize]
        [ProducesResponseType(200, Type = typeof(Result<PaginatedList<QueryDto>>))]
        [ProducesResponseType(400, Type = typeof(ErrorResult))]
        [HttpGet("GetQueryHistory")]
        public async Task<IActionResult> GetQueryHistory([FromQuery] GetQueryHistoryQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(new Result<PaginatedList<QueryDto>>(result, "History of queries returned successfully"));
        }
    }
}
