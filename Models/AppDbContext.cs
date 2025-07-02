using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace StudentManagement.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; } //to query and save instances of student class
        public DbSet<Teacher> Teachers { get; set; } //to query and save instances of teacher class

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();

                foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                {
                    foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
                }
            modelBuilder.Entity<Teacher>()
        .HasOne(t => t.User)
        .WithMany(u => u.Teachers)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
