using Application.Exceptions;
using Application.Interfaces;
using Application.Responses;
using Application.Dto.LoginUserVm;
using Application.Dto.RegisterUserVm;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;

namespace Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public IdentityService(ApplicationDbContext context, IPasswordHasher<User> hasher, AuthenticationSettings authenticationSettings)
        {
            _context = context;
            _hasher = hasher;
            _authenticationSettings = authenticationSettings;
        }

        public Task<bool> AuthorizeAsync(int userId, string policyName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> LoginAsync(LoginUserDto model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

            if(user is null)
            {
                throw new BadRequestException("Podano nieprawidłowe dane");
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Podano nieprawidłowe dane");
            }

            var claims = getClaims(user);
            return generateJwtToken(claims);  
        }

        private string generateJwtToken(List<Claim> claims)
        {          
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireDays = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expireDays,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private List<Claim> getClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.Value.ToString()),
            };
            return claims;
        }    

        public Task<string> GetUserNameAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(int userId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<int> RegisterAsync(RegisterUserDto model)
        {
            var newUser = new User()
            {
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Role = await _context.Roles.FirstOrDefaultAsync(x => x.Name == "User")
            };
            var hashedPassword = _hasher.HashPassword(newUser, model.Password);

            newUser.PasswordHash = hashedPassword;
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser.Id;
            
        }
    }
}
