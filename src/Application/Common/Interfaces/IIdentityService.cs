using Application.Identities.Commands.LoginUser;
using Application.Identities.Commands.RegisterUser;
using Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> Authorize(int userId, string policyName);
        Task<string> GetUserName(int userId);
        Task<int> Register(RegisterUserCommand model);
        Task<bool> IsInRole(int userId, string role);            
        Task DeleteUser(int userId);
        Task<string> Login(LoginUserCommand model);
    }
}
