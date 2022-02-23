
using Application.Common.Exceptions;
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
            string globalMessage = "Something went wrong";
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
                    case ValidationException validationException:
                        code = HttpStatusCode.BadRequest;
                        result = new ErrorResult(validationException.Message, validationException.Errors);
                        break;
                    case AlreadyExistsException alreadyExistsException:
                        code = HttpStatusCode.BadRequest;
                        result = alreadyExistsException.IsPublic ? new ErrorResult(e.Message) : new ErrorResult(globalMessage);
                        break;
                    case NotFoundException notFoundException:
                        code = HttpStatusCode.BadRequest;
                        result = notFoundException.IsPublic ? new ErrorResult(e.Message) : new ErrorResult(globalMessage);
                        break;
                    case BadRequestException badRequestException:
                        code = HttpStatusCode.BadRequest;
                        result = badRequestException.IsPublic ? new ErrorResult(e.Message) : new ErrorResult(globalMessage);
                        break;
                    case ForbidException forbidException:
                        code = HttpStatusCode.Forbidden;
                        result = forbidException.IsPublic ? new ErrorResult(e.Message) : new ErrorResult(globalMessage);
                        break;
                    case ArgumentNullException _:
                        code = HttpStatusCode.InternalServerError;
                        result = new ErrorResult(globalMessage);
                        break;
                    case UnauthorizedAccessException _:
                        code = HttpStatusCode.Unauthorized;
                        result = new ErrorResult("Unauthorized");
                        break;
                    case SqlException sqlException:
                        code = HttpStatusCode.BadRequest;
                        result = new ErrorResult(sqlException.Message);
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
