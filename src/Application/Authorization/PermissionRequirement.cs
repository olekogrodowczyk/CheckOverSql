using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{

    public class PermissionRequirement : IAuthorizationRequirement
    {

        public PermissionRequirement(string permissionTitle)
        {
            PermissionTitle = permissionTitle;
        }

        public string PermissionTitle { get; }
    }
}
