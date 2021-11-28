using Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Behaviours
{
    public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly Stopwatch _stopwatch;
        private readonly IUserContextService _userContextService;
        private readonly ILogger<TRequest> _logger;

        public PerformanceBehaviour(IUserContextService userContextService, ILogger<TRequest> logger)
        {
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _stopwatch.Start();

            var response = await next();

            _stopwatch.Stop();
            var elapsedMiliseconds = _stopwatch.ElapsedMilliseconds;
            if(elapsedMiliseconds > 500)
            {
                var requestName = typeof(TRequest).Name;
                var user = _userContextService.User ?? null;
                string userName = string.Empty;
                int userId = 0;
                if(user is not null)
                {
                    userName = user.FirstName + " " + user.LastName + " " + user.Email;
                    userId = _userContextService.GetUserId ?? 0;
                }

                _logger.LogWarning($"Request with name: {requestName} run too long with {elapsedMiliseconds} miliseconds" +
                    $" by userId - {userId} with name: {userName}");
            }
            return response;
        }
    }
}
