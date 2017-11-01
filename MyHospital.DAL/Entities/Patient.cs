using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Entities
{
    public class Patient : MyHospitalUser
    {
        public int TaxCode { get; set; }
    }
}
