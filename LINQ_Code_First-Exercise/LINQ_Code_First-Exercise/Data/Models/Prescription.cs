using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace P01_HospitalDatabase.Data.Models 
{
   public  class Prescription
    {
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        [ForeignKey("Medicament")]
        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; }
    }
}
