using Application.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class LoggingBehavour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IUserContextService _userContextService;

        public LoggingBehavour(ILogger<TRequest> logger, IUserContextService userContextService)
        {
            _logger = logger;
            _userContextService = userContextService;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var user = _userContextService.User;
            int userId = _userContextService.GetUserId ?? 0;
            string userName = string.Empty;

            if(user is not null)
            {
                userName = user.FirstName + " " + user.LastName + " " + user.Email;
            }

            _logger.LogInformation($"CheckOverSql Request: {userName} {userId} {requestName} {request}");
            return Task.CompletedTask;
        }
    }
}
