using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.DAL.Models
{
    public class Medication : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Dosage { get; set; }

        public string Description { get; set; }

        public DateTime ExpiryDate { get; set; }

        public DateTime ProductionDate { get; set; }


        public ICollection<MedicalExamination> MedicalExaminations { get; set; } = new List<MedicalExamination>();





    }
}
