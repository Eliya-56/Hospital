using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Entities
{
    public class DeseaseStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Desease> Deseases { get; set; }
    }
}
