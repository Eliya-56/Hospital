using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Entities
{
    public class MyHospitalUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public int? Age { get; set; }
        public virtual ICollection<Desease> Deseases { get; set; }

        public MyHospitalUser()
        {
            Deseases = new List<Desease>();
        }

    }
}
