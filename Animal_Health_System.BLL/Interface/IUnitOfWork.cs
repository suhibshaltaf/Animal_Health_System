using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Interface
{
    public interface IUnitOfWork
    {
        UserManager<ApplicationUser> UserManager { get; }

        public IAnimalRepository animalRepository { get; }
        public IBirthRepository birthRepository { get; }
        public IFarmRepository farmRepository { get; }   
        public IFarmStaffRepository farmStaffRepository { get; }
        public IMatingRepository matingRepository { get; }
        public IMedicalExaminationRepository medicalExaminationRepository { get; }
        public IMedicalRecordRepository medicalRecordRepository { get; }
        public IMedicationRepository medicationRepository { get; }
        public IOwnerRepository ownerRepository { get; }
        public IPregnancyRepository pregnancyRepository { get; }
        public IVaccineHistoryRepository vaccineHistoryRepository { get; }
        public IVaccineRepository vaccineRepository { get; }
        public IVeterinarianRepository veterinarianRepository { get; }
        Task SaveChangesAsync();

    }
}
