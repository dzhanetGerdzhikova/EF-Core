using StudentsOrmTest.Models;
using System;
using System.Linq;

namespace StudentsOrmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new StudentDbContext();
            //dbContext.Database.EnsureCreated();
            //dbContext.Courses.Add(new Course { Name = "Entity Framework Core" });
            //dbContext.Courses.Add(new Course { Name = "SQL Server" });
            //dbContext.SaveChanges();

            //dbContext.Grades.Add(new Grade
            //{
            //    Student = new Student { FirstName = "Ivan", LastName = "Petrov" },
            //    Course = new Course { Name = "C# OOP" }
            //});
            //dbContext.SaveChanges();

            var grade = dbContext.Grades.FirstOrDefault(s => s.Student.FirstName == "Ivan");
            grade.Value = 6;
            dbContext.SaveChanges();
        }
    }
}
