using Application.Exceptions;
using Application.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = new ErrorResult();
            try
            {
                await next.Invoke(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                switch (e)
                {
                    case AlreadyExistsException _:
                    case NotFoundException _:
                        code=HttpStatusCode.NotFound;
                        result = new ErrorResult(e.Message);
                        break;
                    case BadRequestException _:
                    case ValidationException _:
                        code = HttpStatusCode.BadRequest;
                        result = new ErrorResult(e.Message);
                        break;
                    case ForbidException _:
                        code = HttpStatusCode.Forbidden;
                        result = new ErrorResult(e.Message);
                        break;
                    case ArgumentNullException _:
                        code = HttpStatusCode.InternalServerError;
                        result = new ErrorResult(e.Message);
                        break;
                    case SqlException _:
                        code = HttpStatusCode.Forbidden;
                        result = new ErrorResult("Unauthorized attempt to use a database");
                        break;
                    case Exception:
                        code = HttpStatusCode.InternalServerError;
                        result = string.IsNullOrWhiteSpace(e.Message) ? new ErrorResult("Error") : new ErrorResult(e.Message);
                        break;
                }
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;

                string jsonResponse = JsonSerializer.Serialize(result);

                await context.Response.WriteAsync(jsonResponse);

            }

        }
    }
}
