using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Animal_Health_System.BLL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly ILoggerFactory loggerFactory;
        public UserManager<ApplicationUser> UserManager { get; }

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

        public UnitOfWork(ApplicationDbContext context, ILoggerFactory loggerFactory, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.loggerFactory = loggerFactory;
            this.UserManager = userManager;


            animalRepository = new AnimalRepository(context, loggerFactory.CreateLogger<AnimalRepository>());
           
            birthRepository = new BirthRepository(context, loggerFactory.CreateLogger<BirthRepository>());
            farmRepository = new FarmRepository(context, loggerFactory.CreateLogger<FarmRepository>());
            farmStaffRepository = new FarmStaffRepository(context, loggerFactory.CreateLogger<FarmStaffRepository>());
          
            matingRepository = new MatingRepository(context, loggerFactory.CreateLogger<MatingRepository>());
            medicalExaminationRepository = new MedicalExaminationRepository(context, loggerFactory.CreateLogger<MedicalExaminationRepository>());
            medicalRecordRepository = new MedicalRecordRepository(context, loggerFactory.CreateLogger<MedicalRecordRepository>());
            medicationRepository = new MedicationRepository(context, loggerFactory.CreateLogger<MedicationRepository>());
            ownerRepository = new OwnerRepository(context, loggerFactory.CreateLogger<OwnerRepository>());
            pregnancyRepository = new PregnancyRepository(context, loggerFactory.CreateLogger<PregnancyRepository>());
         
            vaccineHistoryRepository = new VaccineHistoryRepository(context, loggerFactory.CreateLogger<VaccineHistoryRepository>());
            vaccineRepository = new VaccineRepository(context, loggerFactory.CreateLogger<VaccineRepository>());
            veterinarianRepository = new VeterinarianRepository(context, loggerFactory.CreateLogger<VeterinarianRepository>());
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while saving changes." + ex.Message, ex);
            }
        }
        // Dispose method for clean-up
        public void Dispose()
        {
            context.Dispose();
        }
    }
}
