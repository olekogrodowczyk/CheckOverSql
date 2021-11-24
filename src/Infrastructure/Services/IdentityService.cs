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
using Application.Common.Exceptions;

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

        public Task<bool> Authorize(int userId, string policyName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Login(LoginUserDto model)
        {
            var user = await _context.Users
                .Include(x=>x.Role)
                .FirstOrDefaultAsync(x => x.Email == model.Email);
            if(user is null)
            {
                throw new BadRequestException("Invalid user name or password", true);
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if(result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid user name or passwor", true);
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
                new Claim(ClaimTypes.Role,user.Role.Name)
            };
            return claims;
        }    

        public Task<string> GetUserName(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRole(int userId, string role)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Register(RegisterUserDto model)
        {
            if (_context.Users.Any(x => x.Email == model.Email))
            {
                throw new AlreadyExistsException($"User with defined email: {model.Email} already exists");
            }

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
