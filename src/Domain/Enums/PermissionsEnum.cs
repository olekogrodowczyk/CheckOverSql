using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum PermissionEnum
    {
        [Display(Name = "Sending Invitations")]
        SendingInvitations,
        [Display(Name = "Deleting Users")]
        DeletingUsers,
        [Display(Name = "Deleting Group")]
        DeletingGroup,
        [Display(Name = "Assigning Exercises")]
        AssigningExercises,
        [Display(Name = "Checking Exercises")]
        CheckingExercises,
        [Display(Name = "Getting Assignments")]
        GettingAssignments,
        [Display(Name = "Doing Exercises")]
        DoingExercises,
        [Display(Name = "Changing Roles")]
        ChangingRoles,
    }



}
