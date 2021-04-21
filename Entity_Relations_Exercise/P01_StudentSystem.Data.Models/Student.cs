using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
   public class Student
    {
        public Student()
        {
            this.HomeworkSubmissions = new HashSet<HomeworkSubmission>();
            this.StudentCourses = new HashSet<StudentCourse>();
        }
        public int StudentId { get; set; }
        public string Birthday { get; set; }

        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public bool RefisteredOn { get; set; }

        public virtual ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }
        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
    }
}
