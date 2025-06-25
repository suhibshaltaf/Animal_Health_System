using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Animal_Health_System.DAL.Models
{
    public class Vaccine : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Dose { get; set; }


        public string Description { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime ProductionDate { get; set; }


        public ICollection<VaccineHistory> VaccineHistories { get; set; } = new List <VaccineHistory>();

      
    }
}
