using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.DAL.Models
{

    public class MedicalRecord : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }


        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }

        

        public Animal Animal { get; set; }


        public ICollection<MedicalExamination> Examinations { get; set; } = new List<MedicalExamination>();

        public ICollection<VaccineHistory> vaccineHistories { get; set; } = new List<VaccineHistory>();
    }
}
