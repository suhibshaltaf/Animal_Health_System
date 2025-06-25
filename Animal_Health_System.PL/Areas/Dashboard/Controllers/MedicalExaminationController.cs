using Animal_Health_System.BLL.Interface;
using Animal_Health_System.BLL.Repository;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalExaminationVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class MedicalExaminationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<MedicalExaminationController> logger;

        public MedicalExaminationController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicalExaminationController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var medicalExaminations = await unitOfWork.medicalExaminationRepository.GetAllAsync();
            var medicalExaminationVm = mapper.Map<IEnumerable<MedicalExaminationVM>>(medicalExaminations);
            return View(medicalExaminationVm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var medicalExamination = await unitOfWork.medicalExaminationRepository.GetAsync(id);
            if (medicalExamination == null)
                return NotFound();

            var viewModel = mapper.Map<MedicalExaminationDetailsVM>(medicalExamination);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var vm = new MedicalExaminationFormVM
                {
                    MedicationsList = new SelectList(await unitOfWork.medicationRepository.GetAllAsync(), "Id", "Name"),
                    Veterinarian = new SelectList(await unitOfWork.veterinarianRepository.GetAllAsync(), "Id", "FullName"),
                    Farm = new SelectList(await unitOfWork.farmRepository.GetAllAsync(), "Id", "Name")
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error loading Create page.");
                TempData["Error"] = "An error occurred while loading the page.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalExaminationFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return View(vm);
            }

            try
            {
                var medicalExamination = mapper.Map<MedicalExamination>(vm);
                await unitOfWork.medicalExaminationRepository.AddAsync(medicalExamination);
                await unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Medical examination added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating medical examination.");
                TempData["Error"] = "An error occurred while saving.";
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            logger.LogInformation($"Edit action called with ID: {id}");

            var medicalExamination = await unitOfWork.medicalExaminationRepository.GetAsync(id);
            if (medicalExamination == null)
            {
                TempData["Error"] = "Medical examination not found.";
                return RedirectToAction("Index");
            }

            var vm = mapper.Map<MedicalExaminationFormVM>(medicalExamination);
            vm.MedicationsList = new SelectList(await unitOfWork.medicationRepository.GetAllAsync(), "Id", "Name", vm.SelectedMedications);
            vm.Veterinarian = new SelectList(await unitOfWork.veterinarianRepository.GetAllAsync(), "Id", "FullName", vm.VeterinarianId);
            vm.Farm = new SelectList(await unitOfWork.farmRepository.GetAllAsync(), "Id", "Name", vm.FarmId);
            vm.Animal = new SelectList(await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(vm.FarmId ?? 0), "Id", "Name", vm.AnimalId);
            vm.MedicalRecord = new SelectList(await unitOfWork.medicalRecordRepository.GetByFarmAsync(vm.FarmId ?? 0), "Id", "RecordNumber", vm.MedicalRecordId);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicalExaminationFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill all required fields.";
                return View(vm);
            }

            try
            {
                var medicalExamination = mapper.Map<MedicalExamination>(vm);
                await unitOfWork.medicalExaminationRepository.UpdateAsync(medicalExamination);
                await unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Medical examination updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error updating medical examination." + ex.Message);
                TempData["Error"] = "An error occurred while updating." + ex.Message;
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicalExamination = await unitOfWork.medicalExaminationRepository.GetAsync(id);
            if (medicalExamination == null)
            {
                TempData["Error"] = "Medical examination not found.";
                return RedirectToAction(nameof(Index));
            }

            if (medicalExamination.IsDeleted)
            {
                return BadRequest("The medical examination is already deleted.");
            }

            await unitOfWork.medicalExaminationRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Medical Examination deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetAnimalsByFarm(int farmId)
        {
            var animals = await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(farmId);
            return Json(new SelectList(animals, "Id", "Name"));
        }

        [HttpGet]
        public async Task<JsonResult> GetMedicalRecordsByFarm(int farmId)
        {
            var records = await unitOfWork.medicalRecordRepository.GetByFarmAsync(farmId);
            return Json(new SelectList(records, "Id", "RecordNumber"));
        }

        [HttpGet]
        public async Task<IActionResult> CheckMedicalRecord(int animalId)
        {
            var medicalRecord = await unitOfWork.medicalRecordRepository.GetByAnimalIdAsync(animalId);
            return Json(medicalRecord != null
                ? new { hasMedicalRecord = true, medicalRecordId = medicalRecord.Id, medicalRecordName = medicalRecord.Name }
                : new { hasMedicalRecord = false });
        }
    }
}