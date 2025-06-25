using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmStaffVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MatingVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalRecordVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicationVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.OwnerVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.PregnancyVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineHistoryVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VeterinarianVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Animal_Health_System.PL.Mapping
{
    public class MappingProfile:Profile
    {
        
        public MappingProfile()
        {
            //************  Animal  ***************
            CreateMap<Animal, AnimalVM>().ReverseMap();
            CreateMap<AnimalFormVM, Animal>().ReverseMap();
            CreateMap<Animal, AnimalDetailsVM>().ReverseMap();
          

            //************  Farm  ***************
            CreateMap<Farm, FarmVM>()
      
      .ReverseMap();

            CreateMap<Farm, FarmDetailsVM>()
              
                .ReverseMap();

            CreateMap<FarmFormVM, Farm>().ReverseMap();
           

            //************  FarmStaff  ***************

            CreateMap<FarmStaff, FarmStaffVM>().ReverseMap();
            CreateMap<FarmStaff, FarmStaffFormVM>().ReverseMap();
            CreateMap<FarmStaff, FarmStaffDetailsVM>().ReverseMap();

            // ************** Veterinarian ***********
            CreateMap<Veterinarian, VeterinarianDetailsVM>().ReverseMap();
            CreateMap<Veterinarian, VeterinarianFormVM>().ReverseMap();
            CreateMap<Veterinarian, VeterinarianVM>().ReverseMap();

            //Owner

            CreateMap<Owner, OwnerVM>().ReverseMap();
            CreateMap<OwnerFormVM, Owner>().ReverseMap();
            CreateMap<Owner, OwnerDetailsVM>().ReverseMap();
            //************  MedicalRecord  ***************

            CreateMap<MedicalRecordFormVM, MedicalRecord>()
    .ForMember(dest => dest.AnimalId, opt => opt.MapFrom(src => src.AnimalId.Value))
    .ReverseMap();
            CreateMap<MedicalRecord, MedicalRecordVM>()
                .ForMember(dest => dest.Farm, opt => opt.MapFrom(src => src.Animal.Farm));
CreateMap<MedicalRecord, MedicalRecordDetailsVM>()
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
    .ReverseMap();
            //************  Medication  ***************

            CreateMap<Medication, MedicationVM>().ReverseMap();
            CreateMap<Medication, MedicationFormVM>().ReverseMap();
            CreateMap<Medication, MedicationDetailsVM>().ReverseMap();

            //************  Vaccine  ***************

            CreateMap<Vaccine, VaccineVM>().ReverseMap();
            CreateMap<Vaccine, VaccineFormVM>().ReverseMap();
            CreateMap<Vaccine, VaccineDetailsVM>().ReverseMap();

            //************  VaccineHistory  ***************

            CreateMap<VaccineHistory, VaccineHistoryVM>().ReverseMap();
            CreateMap<VaccineHistory, VaccineHistoryFormVM>()
              .ForMember(dest => dest.Vaccines, opt => opt.MapFrom(src =>
                  src.vaccine != null ? new List<SelectListItem>
                  {
            new SelectListItem { Text = src.vaccine.Name, Value = src.vaccine.Id.ToString() }
                  } : new List<SelectListItem>())) // Ensure this correctly handles the `vaccine` object
              .ForMember(dest => dest.Veterinarians, opt => opt.MapFrom(src =>
                  new List<SelectListItem>
                  {
            new SelectListItem { Text = src.veterinarian.FullName, Value = src.veterinarian.Id.ToString() }
                  }))
              .ForMember(dest => dest.MedicalRecords, opt => opt.MapFrom(src =>
                  src.medicalRecord != null ? new List<SelectListItem>
                  {
            new SelectListItem { Text = src.medicalRecord.Name, Value = src.medicalRecord.Id.ToString() }
                  } : new List<SelectListItem>())) // Ensure this handles medicalRecord
              .ReverseMap();


            CreateMap<VaccineHistory, VaccineHistoryDetailsVM>().ReverseMap();

            //************  MedicalExamination  ***************

            CreateMap<MedicalExamination, MedicalExaminationVM>().ReverseMap();

            CreateMap<MedicalExamination, MedicalExaminationFormVM>()
                .ForMember(dest => dest.Animal, opt => opt.Ignore()) // تجنب محاولة تحويل Animal إلى SelectList
                .ForMember(dest => dest.MedicalRecord, opt => opt.Ignore()) // نفس المشكلة مع MedicalRecord
                .ForMember(dest => dest.MedicationsList, opt => opt.Ignore()) // تجاهل SelectList الأخرى
                .ForMember(dest => dest.Veterinarian, opt => opt.Ignore())
                .ForMember(dest => dest.Farm, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<MedicalExamination, MedicalExaminationDetailsVM>()
                
                .ReverseMap();

            //************  Medication  ***************

            CreateMap<Mating, MatingVM>()
      .ForMember(dest => dest.Farm, opt => opt.MapFrom(src => src.Farm)) // التأكد من تضمين الـ Farm
      .ReverseMap();



            CreateMap<Mating, MatingFormVM>()
      .ForMember(dest => dest.MaleAnimalId, opt => opt.MapFrom(src => src.MaleAnimalId))
      .ForMember(dest => dest.FemaleAnimalId, opt => opt.MapFrom(src => src.FemaleAnimalId))
      .ForMember(dest => dest.FarmId, opt => opt.MapFrom(src => src.FarmId))
      .ReverseMap();


            CreateMap<Mating, MatingDetailsVM>()
                .ForMember(dest => dest.MaleAnimal, opt => opt.MapFrom(src => src.MaleAnimal))
                .ForMember(dest => dest.FemaleAnimal, opt => opt.MapFrom(src => src.FemaleAnimal))
                .ForMember(dest => dest.Farm, opt => opt.MapFrom(src => src.Farm))
                .ReverseMap();

            //************  Pregnancy  ***************

            CreateMap<Pregnancy, PregnancyVM>()
     .ForMember(dest => dest.PregnancyDurationText, opt => opt.MapFrom(src => Pregnancy.CalculatePregnancyDuration(src)))
     .ForMember(dest => dest.TimeToBirthText, opt => opt.MapFrom(src => Pregnancy.CalculateTimeToBirth(src.ExpectedBirthDate)))
     .ReverseMap();

            // PregnancyFormVM -> Pregnancy (For Create/Update)
            CreateMap<PregnancyFormVM, Pregnancy>()
                .ForMember(dest => dest.ExpectedBirthDate, opt => opt.MapFrom(src => src.ExpectedBirthDate))
                .ForMember(dest => dest.MatingDate, opt => opt.MapFrom(src => src.MatingDate));

            // Pregnancy -> PregnancyFormVM (For editing)
            CreateMap<Pregnancy, PregnancyFormVM>()
                .ForMember(dest => dest.ExpectedBirthDate, opt => opt.MapFrom(src => src.ExpectedBirthDate))
                .ForMember(dest => dest.MatingDate, opt => opt.MapFrom(src => src.MatingDate));

            // Pregnancy -> PregnancyDetailsVM (For details)
            CreateMap<Pregnancy, PregnancyDetailsVM>()
            .ForMember(dest => dest.PregnancyDurationText, opt => opt.MapFrom(src => Pregnancy.CalculatePregnancyDuration(src)))
            .ForMember(dest => dest.TimeToBirthText, opt => opt.MapFrom(src => Pregnancy.CalculateTimeToBirth(src.ExpectedBirthDate)))
            .ForMember(dest => dest.Births, opt => opt.MapFrom(src => src.Births));


            //************  Birth  ***************

            CreateMap<Birth, BirthVM>().ReverseMap();
            CreateMap<Birth, BirthFormVM>()
      .ForMember(dest => dest.Pregnancy, opt => opt.Ignore()) // Ignore Pregnancy during mapping
      .ForMember(dest => dest.Animal, opt => opt.Ignore())   // Ignore Animal during mapping
      .ReverseMap() // Configure reverse mapping (BirthFormVM -> Birth)
      .ForMember(dest => dest.Pregnancy, opt => opt.Ignore()) // Ignore Pregnancy during reverse mapping
      .ForMember(dest => dest.Animal, opt => opt.Ignore());   // Ignore Animal during reverse mapping

            CreateMap<Birth, BirthDetailsVM>().ReverseMap();


        }
    }
}
