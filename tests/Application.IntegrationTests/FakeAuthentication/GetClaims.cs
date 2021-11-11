using System;
using System.Security.Claims;

namespace WebAPI.IntegrationTests.FakeAuthentication
{
    public static class GetClaims
    {
        public static Claim[] getClaims()
        {
            return new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, 99.ToString()),
                    new Claim(ClaimTypes.Role,"User"),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.UtcNow.AddYears(-20).ToString()),
                    new Claim(ClaimTypes.Name,"Fake User"),
                    new Claim(ClaimTypes.Email,"testfakeuser@gmail.com")
                };
        }
    }
}
