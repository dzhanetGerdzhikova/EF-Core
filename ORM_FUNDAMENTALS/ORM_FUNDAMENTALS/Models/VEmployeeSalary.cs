using System;
using System.Collections.Generic;

#nullable disable

namespace ORM_FUNDAMENTALS.Models
{
    public partial class VEmployeeSalary
    {
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public decimal Salary { get; set; }
    }
}
