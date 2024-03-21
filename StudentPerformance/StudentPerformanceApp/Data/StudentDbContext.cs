
using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Data.Models;
using System.Reflection.Metadata;

namespace StudentPerformanceApp.Data
{
    public class StudentDbContext : DbContext
    {

        public DbSet<Models.Grades> Grades { get; set; }
        public DbSet<Models.Student> Students { get; set; }
        public DbSet<Models.School> Schools { get; set; }
        public DbSet<Models.Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql("Host=localhost;Username=postgres;Password=password;Database=net_student_performance");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(b => b.Name)
                .HasDefaultValueSql("\"left\"(concat('Student_', uuid_generate_v4()), 16)");
        }

    }
}