using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Entities
{
    public class Doctor : MyHospitalUser
    {
        public string Specialization { get; set; }
    }
}
