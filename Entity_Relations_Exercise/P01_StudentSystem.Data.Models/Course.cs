using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
    public class Course
    {
        public Course()
        {
            this.StudentCourses = new HashSet<StudentCourse>();
            this.Resources = new HashSet<Resource>();
            this.HomeworkSubmissions = new HashSet<HomeworkSubmission>();
        }
        public int CourseId { get; set; }
        public string Description { get; set; }
        public DateTime EndDate { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }

        public virtual ICollection<StudentCourse> StudentCourses { get; set; }
        public virtual ICollection<Resource> Resources { get; set; }
        public virtual ICollection<HomeworkSubmission> HomeworkSubmissions { get; set; }

        
    }
}
