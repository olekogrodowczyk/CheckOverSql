using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.IntegrationTests.FakeAuthentication;

namespace Application.IntegrationTests.FakeAuthentication
{
    public class FakeUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            claimsPrincipal.AddIdentity(new ClaimsIdentity(
                GetClaims.getClaims()
                ));

            context.HttpContext.User = claimsPrincipal;

            await next();
                
        }
    }
}
