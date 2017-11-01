using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyHospital.Web.Models
{
    public class CreateDeseaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Введите вашу жалобу")]
        public string Complaint { get; set; }
        [Required]
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
    }
}