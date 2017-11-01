using Microsoft.AspNet.Identity.EntityFramework;
using MyHospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.EF
{
    public class MyHospitalContext : IdentityDbContext<MyHospitalUser>
    {
        public MyHospitalContext(string connectionString) : base(connectionString) { }

        public DbSet<Desease> Deseases { get; set; }
        public DbSet<DeseaseStatus> DeseaseStatuses { get; set; }
        public DbSet<DeseaseRecord> DeseaseRecords { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>()
            .HasMany(p => p.Deseases)
            .WithRequired(p => p.Doctor)
            .HasForeignKey(s => s.PatientId);
            modelBuilder.Entity<Patient>()
            .HasMany(p => p.Deseases)
            .WithRequired(p => p.Patient)
            .HasForeignKey(s => s.DoctorId);
            modelBuilder.Entity<DeseaseStatus>()
            .HasMany(p => p.Deseases)
            .WithRequired(p => p.Status)
            .HasForeignKey(s => s.StatusId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
