using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {

        }
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;DataBase=Hospital;Integrated Security=true");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(patient =>
            {
                patient.Property(p => p.FirstName).IsUnicode(true);
                patient.Property(p => p.LastName).IsUnicode(true);
                patient.Property(p => p.Address).IsUnicode(true);
                patient.Property(p => p.Email).IsUnicode(false);
            });

            modelBuilder.Entity<Visitation>(visitation =>
            {
                visitation.Property(v => v.Comments).IsUnicode(true);
            });

            modelBuilder.Entity<Diagnose>(diagnose =>
            {
                diagnose.Property(v => v.Name).IsUnicode(true);
                diagnose.Property(v => v.Comments).IsUnicode(true);
            });

            modelBuilder.Entity<Medicament>(medicament =>
            {
                medicament.Property(v => v.Name).IsUnicode(true);
            });

            modelBuilder.Entity<Prescription>(prescription =>
                {
                    prescription.HasKey(p => new { p.MedicamentId, p.PatientId });
                });
        }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Visitation> Visitations { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }

    }
}
