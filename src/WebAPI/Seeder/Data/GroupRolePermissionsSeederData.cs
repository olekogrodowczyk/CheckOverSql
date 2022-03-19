using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Seeders
{
    public static class GroupRolePermissionsSeederData
    {
        public static IEnumerable<GroupRolePermission> GetGroupRolePermissions(IEnumerable<Permission> permissions, IEnumerable<GroupRole> groupRoles)
        {
            GroupRolePermission getGroupRolePermission(string groupRoleName, string permission)
            {
                return new GroupRolePermission
                {
                    GroupRoleId = groupRoles.SingleOrDefault(x => x.Name == groupRoleName).Id,
                    PermissionId = permissions.SingleOrDefault(x => x.Title == permission).Id
                };
            }
            return new List<GroupRolePermission>
            {
                getGroupRolePermission("Owner", PermissionEnum.SendingInvitations.GetDisplayName()),
                getGroupRolePermission("Owner", PermissionEnum.DeletingUsers.GetDisplayName()),
                getGroupRolePermission("Owner", PermissionEnum.DeletingGroup.GetDisplayName()),
                getGroupRolePermission("Owner", PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Owner", PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Owner", PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.SendingInvitations.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.DeletingUsers.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Moderator", PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("Checker", PermissionEnum.AssigningExercises.GetDisplayName()),
                getGroupRolePermission("Checker", PermissionEnum.CheckingExercises.GetDisplayName()),
                getGroupRolePermission("Checker", PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("User", PermissionEnum.GettingAssignments.GetDisplayName()),
                getGroupRolePermission("User", PermissionEnum.DoingExercises.GetDisplayName())
            };
        }
    }
}
