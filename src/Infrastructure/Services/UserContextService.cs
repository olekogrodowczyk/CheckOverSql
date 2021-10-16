using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public UserContextService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public ClaimsPrincipal UserClaimPrincipal => _httpContextAccessor.HttpContext?.User;
        public int? GetUserId => UserClaimPrincipal is null ? null : (int?)int.Parse(UserClaimPrincipal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
        public User User => _context.Users.FirstOrDefault(x => x.Id == GetUserId);
        public bool isUserLoggedIn => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

    }
}
