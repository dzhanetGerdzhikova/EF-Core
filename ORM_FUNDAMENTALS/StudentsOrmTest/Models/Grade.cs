using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsOrmTest.Models
{
   public  class Grade
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
