using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Authorization
{

    public class PermissionRequirement : IAuthorizationRequirement
    {

        public PermissionRequirement(PermissionNames permissionName)
        {
            PermissionTitle = GetPermissionByEnum.GetPermissionName(permissionName);
        }
        public string PermissionTitle { get; }
    }
}
