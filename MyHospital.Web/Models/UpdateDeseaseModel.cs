using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyHospital.Web.Models
{
    public class UpdateDeseaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Complaint { get; set; }
        public string DoctorName { get; set; }
        public string DoctorId { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }
}