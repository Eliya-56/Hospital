using MyHospital.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.BLL.DTO
{
    public class DeseaseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Complaint { get; set; }
        public string Status { get; set; }
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<DeseaseRecordDTO> Records { get; set; }
        public DeseaseDTO()
        {
            Records = new List<DeseaseRecordDTO>();
        }
    }
}
