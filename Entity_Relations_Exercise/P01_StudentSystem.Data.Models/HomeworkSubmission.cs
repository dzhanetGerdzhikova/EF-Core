using P01_StudentSystem.Data.Models.ContentType;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Models
{
   public class HomeworkSubmission
    {
        public int HomeworkId { get; set; }
        public string Content { get; set; }
        public ContentEnum ContentType  { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public DateTime SubmissionTime { get; set; }

    }
}
