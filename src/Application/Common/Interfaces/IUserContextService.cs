using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal UserClaimPrincipal { get; }
        User User { get; }
        bool isUserLoggedIn { get; }
    }
}