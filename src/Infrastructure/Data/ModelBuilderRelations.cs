﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class ModelBuilderRelations
    {
        public static void InitializeRelations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Solving>()
                .HasOne<Checking>(x => x.Checking)
                .WithOne(x => x.Solving)
                .HasForeignKey<Checking>(x => x.SolvingId);

            modelBuilder.Entity<Assignment>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne<User>(x => x.Sender)
                .WithMany(x => x.InvitationsSent)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invitation>()
                .HasOne<User>(x => x.Receiver)
                .WithMany(x => x.InvitationsReceived)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exercise>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.Exercises)
                .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<Checking>()
                .HasOne<User>(x => x.Checker)
                .WithMany(x => x.Checkings)
                .HasForeignKey(x => x.CheckerId);

            modelBuilder.Entity<Invitation>()
                .HasOne<Group>(x => x.Group)
                .WithMany(x => x.Invitations)
                .HasForeignKey(x => x.GroupId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Assignment>()
                .HasOne<Group>(x => x.Group)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Assignment>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.GroupRoleId);

            modelBuilder.Entity<Invitation>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.Invitations)
                .HasForeignKey(x => x.GroupRoleId);

            modelBuilder.Entity<Solving>()
                .HasOne<Exercise>(x => x.Exercise)
                .WithMany(x => x.Solvings)
                .HasForeignKey(x => x.ExerciseId);


            modelBuilder.Entity<Solving>()
                .HasOne<Assignment>(x => x.Assignment)
                .WithMany(x => x.Solvings)
                .HasForeignKey(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GroupRolePermission>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.GroupRoleId);

            modelBuilder.Entity<GroupRolePermission>()
                .HasOne<Permission>(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId);

            modelBuilder.Entity<Solving>()
                .HasOne<Solution>(x => x.Solution)
                .WithMany(x => x.Solvings)
                .HasForeignKey(x => x.SolutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solution>()
                .HasOne<Exercise>(x => x.Exercise)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.ExerciseId);

            modelBuilder.Entity<Solution>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.CreatorId);

            modelBuilder.Entity<Solution>()
                .HasOne<Comparison>(x => x.Comparison)
                .WithOne(x => x.Solution)
                .HasForeignKey<Comparison>(x => x.SolutionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comparison>()
                .HasOne<Exercise>(x => x.Exercise)
                .WithMany(x => x.Comparisons)
                .HasForeignKey(x => x.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Solving>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.SolvingsCreated)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Query>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.Queries)
                .HasForeignKey(x => x.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Query>()
                .HasOne<Database>(x => x.Database)
                .WithMany(x => x.Queries)
                .HasForeignKey(x => x.DatabaseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Exercise>()
                .HasOne<Database>(X => X.Database)
                .WithMany(x => x.Exercises)
                .HasForeignKey(x => x.DatabaseId)
                .OnDelete(DeleteBehavior.SetNull);
        }

    }
}
