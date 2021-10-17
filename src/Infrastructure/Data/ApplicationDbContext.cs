using Application.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Checking> Checkings { get; set; }
        public DbSet<Solving> Solvings { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Database> Databases { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region builders

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

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
                .HasOne<User>(x=>x.Sender)
                .WithMany(x=>x.InvitationsSent)
                .HasForeignKey(x=> x.SenderId)
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
                .HasForeignKey(x => x.GroupId);

            modelBuilder.Entity<Assignment>()
                .HasOne<Group>(x => x.Group)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.GroupId);

            modelBuilder.Entity<Assignment>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Invitation>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.Invitations)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<Solving>()
                .HasOne<Exercise>(x => x.Exercise)
                .WithMany(x => x.Solvings)
                .HasForeignKey(x => x.ExerciseId);

            modelBuilder.Entity<Solving>()
                .HasOne<Assignment>(x => x.Assignment)
                .WithMany(x => x.Solvings)
                .HasForeignKey(x => x.AssignmentId);

            modelBuilder.Entity<RolePermission>()
                .HasOne<GroupRole>(x => x.GroupRole)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne<Permission>(x => x.Permission)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(x => x.PermissionId);

            modelBuilder.Entity<Solving>()
                .HasOne<Solution>(x => x.Solution)
                .WithOne(x => x.Solving)
                .HasForeignKey<Solution>(x => x.SolvingId);

            modelBuilder.Entity<Solution>()
                .HasOne<Exercise>(x => x.Exercise)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.ExerciseId);

            modelBuilder.Entity<Solution>()
                .HasOne<User>(x => x.Creator)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.CreatorId);


            #endregion
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entries)
            {
                ((AuditableEntity)entity.Entity).LastModified = DateTime.UtcNow;

                if(entity.State==EntityState.Added)
                {
                    ((AuditableEntity)entity.Entity).Created = DateTime.UtcNow;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }



        
    }
}
