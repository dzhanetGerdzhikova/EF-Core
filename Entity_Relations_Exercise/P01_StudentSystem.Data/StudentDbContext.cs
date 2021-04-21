using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data
{
   public class StudentDbContext : DbContext
    {
        public StudentDbContext()
        {

        }

       public StudentDbContext(DbContextOptions<StudentDbContext> options)
            :base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConfigurationString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(student =>
            {
                student.HasKey(student => student.StudentId);
                student.Property(student => student.Name).HasMaxLength(100).IsUnicode(true).IsRequired(true);
                student.Property(student => student.PhoneNumber).HasMaxLength(10).IsUnicode(false);
                student.Property(student => student.Birthday).IsRequired(false);
            });

            modelBuilder.Entity<Course>(course =>
            {
                course.HasKey(course => course.CourseId);
                course.Property(c => c.Name).HasMaxLength(80).IsUnicode(true).IsRequired(true);
                course.Property(c => c.Description).IsUnicode(true).IsRequired(false);

            });

            modelBuilder.Entity<Resource>(resource =>
            {
                resource.HasKey(resource => resource.ResourceId);
                resource.Property(r => r.Name).HasMaxLength(50).IsUnicode(true).IsRequired(true);
                resource.Property(r => r.Url).IsUnicode(false).IsRequired(true);

                resource.HasOne(r => r.Course).WithMany(c => c.Resources).HasForeignKey(r => r.CourseId);
            });

            modelBuilder.Entity<HomeworkSubmission>(homeworkSubmission =>
            {
                homeworkSubmission.HasKey(hs => hs.HomeworkId);
                homeworkSubmission.Property(hs => hs.Content).IsUnicode(false).IsRequired(false);

                homeworkSubmission
                .HasOne(hs => hs.Course)
                .WithMany(c => c.HomeworkSubmissions)
                .HasForeignKey(hs => hs.CourseId);

                homeworkSubmission
                .HasOne(hs => hs.Student)
                .WithMany(s => s.HomeworkSubmissions)
                .HasForeignKey(hs => hs.StudentId);
            });

            modelBuilder.Entity<StudentCourse>(sc =>
            {
                sc.HasKey(sc => new { sc.StudentId, sc.CourseId });

                sc
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId);

                sc
                .HasOne(sc => sc.Course)
                .WithMany(c => c.StudentCourses)
                .HasForeignKey(sc => sc.CourseId);
            });
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<HomeworkSubmission> HomeworkSubmission { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }

    }
}
