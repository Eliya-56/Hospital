using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHospital.DAL.Entities
{

    public class Desease
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Complaint { get; set; }

        [ForeignKey("StatusId")]
        public DeseaseStatus Status { get; set; }
        public int? StatusId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor Doctor { get; set; }
        public string DoctorId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
        public string PatientId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ICollection<DeseaseRecord> DeseaseRecords { get; set; }

        public Desease()
        {
            DeseaseRecords = new List<DeseaseRecord>();
        }
    }
}