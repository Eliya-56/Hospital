using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHospital.DAL.Entities
{
    public class DeseaseRecord
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public int DeseaseId { get; set; }
        public virtual Desease Desease { get; set; }
    }
}
