using SoftJail.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SoftJail.Data.Models
{
   public  class Officer
    {
        public Officer()
        {
            OfficersPrisoners = new HashSet<OfficerPrisoner>();
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public Position Position{ get; set; }

        [Required]
        public Weapon 	Weapon  { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }

        [Required]
        public Department Department { get; set; }

        public virtual ICollection<OfficerPrisoner> OfficersPrisoners  { get; set; }
    }
}
