using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsOrmTest.Models
{
   public  class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public int Age { get; set; }
    }
}
