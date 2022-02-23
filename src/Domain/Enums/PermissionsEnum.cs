using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PermissionNames
    {
        SendingInvitations,
        DeletingUsers,
        DeletingGroup,
        AssigningExercises,
        CheckingExercises,
        GettingAssignments,
        DoingExercises,
    }

    public static class GetPermissionByEnum
    {
        public static string GetPermissionName(PermissionNames permissionNames)
        {
            switch (permissionNames)
            {
                case PermissionNames.SendingInvitations: return "Sending Invitations";
                case PermissionNames.DeletingGroup: return "Deleting Group";
                case PermissionNames.DeletingUsers: return "Deleting Users";
                case PermissionNames.AssigningExercises: return "Assigning Exercises";
                case PermissionNames.CheckingExercises: return "Checking Exercises";
                case PermissionNames.GettingAssignments: return "Getting Assignments";
                case PermissionNames.DoingExercises: return "Doing Exercises";
                default: throw new ArgumentException("Error in getPermissionByEnum");
            }
        }
    }

}
