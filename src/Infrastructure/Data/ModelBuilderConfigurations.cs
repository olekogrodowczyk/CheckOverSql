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
                
        }
    }
}
