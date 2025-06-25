using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animal_Health_System.DAL.Models
{
    public class MedicalExamination : EntityBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime ExaminationDate { get; set; }

        public string Diagnosis { get; set; }

        public string Treatment { get; set; }

        public string ExaminationType { get; set; }

        [ForeignKey(nameof(Animal))]
        public int AnimalId { get; set; }

        [ForeignKey(nameof(MedicalRecord))]
        public int MedicalRecordId { get; set; }

        [ForeignKey(nameof(Veterinarian))]
        public int VeterinarianId { get; set; }

        public Animal Animal { get; set; }

        public MedicalRecord MedicalRecord { get; set; }

        public Veterinarian Veterinarian { get; set; }

        public ICollection<Medication> Medications { get; set; } = new HashSet<Medication>();  

    }
}
