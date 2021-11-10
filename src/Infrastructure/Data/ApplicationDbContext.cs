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
        public DbSet<GroupRolePermission> GroupRolePermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Database> Databases { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Comparison> Comparisons { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            ModelBuilderRelations.InitializeRelations(modelBuilder);
            ModelBuilderConfigurations.InitializeConfigurations(modelBuilder);
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
