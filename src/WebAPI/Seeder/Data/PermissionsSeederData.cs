using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;

namespace WebAPI
{
    public static class PermissionsSeederData
    {
        public static IEnumerable<Permission> GetPermissions()
        {
            return new List<Permission>
            {
                new Permission()
                {
                    Title = PermissionEnum.SendingInvitations.GetDisplayName(),
                    Description = "This permission lets a user to send invitations to other users"
                },
                new Permission()
                {
                    Title = PermissionEnum.DeletingUsers.GetDisplayName(),
                    Description = "This permission lets a user to delete other users from group"
                },
                new Permission()
                {
                    Title = PermissionEnum.DeletingGroup.GetDisplayName(),
                    Description = "This permission lets a user to delete a group"
                },
                new Permission()
                {
                    Title = PermissionEnum.AssigningExercises.GetDisplayName(),
                    Description = "This permission lets a user to assign exercises to do"
                },
                new Permission()
                {
                    Title = PermissionEnum.CheckingExercises.GetDisplayName(),
                    Description = "This permission lets a user in group to check other exercises"
                },
                new Permission()
                {
                    Title = PermissionEnum.GettingAssignments.GetDisplayName(),
                    Description = "This permission lets a user to get assignments in group"
                },
                new Permission()
                {
                    Title = PermissionEnum.DoingExercises.GetDisplayName(),
                    Description = "This permission lets a user to do an exercise",
                },
                new Permission()
                {
                    Title = PermissionEnum.ChangingRoles.GetDisplayName(),
                    Description = "This permission lets a user to change other users' roles",
                }
            };
        }
    }
}