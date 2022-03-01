using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public static class ModelBuilderConfigurations
    {
        public static void InitializeConfigurations(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Login)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(x => x.FirstName)
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(x => x.LastName)
                .HasMaxLength(25);

            modelBuilder.Entity<User>()
                .Property(x => x.Email)
                .HasMaxLength(25);

            modelBuilder.Entity<Database>()
                .OwnsOne(c => c.ConnectionString, x =>
                  {
                      x.Property(p => p.Server).HasMaxLength(50)
                      .HasColumnName("Server");
                      x.Property(p => p.Database).HasMaxLength(50)
                      .HasColumnName("Database");
                      x.Property(p => p.User).HasMaxLength(50)
                      .HasColumnName("User");
                      x.Property(p => p.Password).HasMaxLength(50)
                      .HasColumnName("Password");
                  });              
                
        }
    }
}
