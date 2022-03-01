using Domain.Entities;
using Domain.Enums;
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

        public PermissionRequirement(PermissionEnum permission)
        {
            Permission = permission;
        }
        public PermissionEnum Permission { get; }
         
    }
}
