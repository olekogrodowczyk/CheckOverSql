using Application.Responses;
using Application.Dto.LoginUserVm;
using Application.Dto.RegisterUserVm;
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
        Task<int> Register(RegisterUserDto model);
        Task<bool> IsInRole(int userId, string role);            
        Task DeleteUser(int userId);
        Task<string> Login(LoginUserDto model);
    }
}
