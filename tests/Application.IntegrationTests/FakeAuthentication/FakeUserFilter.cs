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
                new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, FakeUserId.Value.ToString()),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.UtcNow.AddYears(-20).ToString()),
                    new Claim(ClaimTypes.Name,"FirstNameTest LastNameTest")
                }));

            context.HttpContext.User = claimsPrincipal;

            await next();
                
        }
    }
}
