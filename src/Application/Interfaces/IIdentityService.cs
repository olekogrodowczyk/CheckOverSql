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
        Task<bool> AuthorizeAsync(int userId, string policyName);
        Task<string> GetUserNameAsync(int userId);
        Task<int> RegisterAsync(RegisterUserDto model);
        Task<bool> IsInRoleAsync(int userId, string role);            
        Task DeleteUserAsync(int userId);
        Task<string> LoginAsync(LoginUserDto model);
    }
}
