using Animal_Health_System.BLL.Interface;
using Animal_Health_System.BLL.Repository;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicalRecordVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class MedicalRecordController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<MedicalRecordController> logger;

        public MedicalRecordController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicalRecordController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var medicalRecords = await unitOfWork.medicalRecordRepository.GetAllAsync();
                var medicalRecordVMs = mapper.Map<IEnumerable<MedicalRecordVM>>(medicalRecords);
                return View(medicalRecordVMs);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical records.");
                TempData["ErrorMessage"] = "An error occurred while retrieving medical records.";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                var vm = new MedicalRecordFormVM
                {
                    Farms = new SelectList(farms, "Id", "Name")
                };
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while loading create form.");
                TempData["ErrorMessage"] = "An error occurred while loading the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicalRecordFormVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // تحقق من وجود الحيوان
                    var animal = await unitOfWork.animalRepository.GetAsync(vm.AnimalId.Value);
                    if (animal == null)
                    {
                        TempData["ErrorMessage"] = "The selected animal does not exist.";
                        return RedirectToAction(nameof(Create));
                    }

                    // تحقق من وجود اسم السجل الطبي
                    var existingName = await unitOfWork.medicalRecordRepository.AnyAsync(r => r.Name == vm.Name);
                    if (existingName)
                    {
                        TempData["ErrorMessage"] = "A medical record with this name already exists.";
                        return RedirectToAction(nameof(Create));
                    }

                    // تحقق من وجود سجل طبي للحيوان
                    var existingRecord = await unitOfWork.medicalRecordRepository.AnyAsync(r => r.AnimalId == vm.AnimalId);
                    if (existingRecord)
                    {
                        TempData["ErrorMessage"] = "This animal already has a medical record.";
                        return RedirectToAction(nameof(Create));
                    }

                    // إنشاء السجل الطبي
                    var medicalRecord = mapper.Map<MedicalRecord>(vm);
                    medicalRecord.AnimalId = vm.AnimalId.Value;

                    await unitOfWork.medicalRecordRepository.AddAsync(medicalRecord);

                    // تعيين MedicalRecordId للحيوان
                    animal.MedicalRecordId = medicalRecord.Id;
                    await unitOfWork.animalRepository.UpdateAsync(animal);

                    await unitOfWork.animalRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Medical record created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                // إذا كانت البيانات غير صالحة، إعادة تحميل النموذج بالبيانات المطلوبة
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = new SelectList(farms, "Id", "Name", vm.FarmId);

                var animals = await unitOfWork.animalRepository.GetAllAsync();
                vm.Animals = new SelectList(animals.Where(a => a.FarmId == vm.FarmId), "Id", "Name", vm.AnimalId);

                return View(vm);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ بالتفاصيل
                logger.LogError(ex, "Error occurred while creating medical record. Data: {Data}", new
                {
                    vm.Name,
                    vm.AnimalId,
                    vm.FarmId
                });
                TempData["ErrorMessage"] = "An error occurred while creating the medical record.";
                return RedirectToAction(nameof(Create));
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var medicalRecord = await unitOfWork.medicalRecordRepository.GetAsync(id);
                if (medicalRecord == null)
                {
                    return NotFound();
                }

                var vm = mapper.Map<MedicalRecordFormVM>(medicalRecord);
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = new SelectList(farms, "Id", "Name", medicalRecord.Animal.FarmId);

                var animals = await unitOfWork.animalRepository.GetAllAsync();
                vm.Animals = new SelectList(animals.Where(a => a.FarmId == medicalRecord.Animal.FarmId), "Id", "Name", medicalRecord.AnimalId);

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while loading edit form.");
                TempData["ErrorMessage"] = "An error occurred while loading the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicalRecordFormVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the name already exists (excluding the current record)
                    var existingName = await unitOfWork.medicalRecordRepository.AnyAsync(r => r.Name == vm.Name && r.Id != vm.Id);
                    if (existingName)
                    {
                        TempData["ErrorMessage"] = "A medical record with this name already exists.";
                        return RedirectToAction(nameof(Edit), new { id = vm.Id });
                    }

                    var medicalRecord = await unitOfWork.medicalRecordRepository.GetAsync(vm.Id);
                    if (medicalRecord == null)
                    {
                        return NotFound();
                    }

                    medicalRecord.Name = vm.Name;
                    medicalRecord.AnimalId = vm.AnimalId.Value;

                    await unitOfWork.medicalRecordRepository.UpdateAsync(medicalRecord);

                    var animal = await unitOfWork.animalRepository.GetAsync(vm.AnimalId.Value);
                    if (animal != null)
                    {
                        animal.MedicalRecordId = medicalRecord.Id;
                        await unitOfWork.animalRepository.UpdateAsync(animal);
                    }

                    await unitOfWork.animalRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Medical record updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = new SelectList(farms, "Id", "Name", vm.FarmId);

                var animals = await unitOfWork.animalRepository.GetAllAsync();
                vm.Animals = new SelectList(animals.Where(a => a.FarmId == vm.FarmId), "Id", "Name", vm.AnimalId);

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating medical record.");
                TempData["ErrorMessage"] = "An error occurred while updating the medical record.";
                return RedirectToAction(nameof(Edit), new { id = vm.Id });
            }
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var medicalRecord = await unitOfWork.medicalRecordRepository.GetAsync(id);
                if (medicalRecord == null)
                {
                    return NotFound();
                }

                var medicalRecordModel = mapper.Map<MedicalRecordDetailsVM>(medicalRecord);
                return View(medicalRecordModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving medical record details.");
                TempData["ErrorMessage"] = "An error occurred while retrieving medical record details.";
                return RedirectToAction(nameof(Index));
            }
        }
        // POST: Dashboard/MedicalRecord/DeleteConfirmed/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var animal = await unitOfWork.medicalRecordRepository.GetAsync(id);
                if (animal == null)
                {
                    return Json(new { success = false, message = "medicalRecord not found." });
                }

                await unitOfWork.medicalRecordRepository.DeleteAsync(id);
                return Json(new { success = true, message = "medicalRecord deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the medicalRecord.");
                return Json(new { success = false, message = "An error occurred while deleting the medicalRecord." });
            }
        }
    
        [HttpGet]
        public async Task<IActionResult> GetAnimalsByFarm(int farmId)
        {
            try
            {
                var animals = await unitOfWork.animalRepository.GetAllAsync();
                var filteredAnimals = animals.Where(a => a.FarmId == farmId).Select(a => new { a.Id, a.Name }).ToList();
                return Json(filteredAnimals);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving animals by farm.");
                return Json(new { error = "An error occurred while retrieving animals." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> CheckAnimalMedicalRecord(int animalId)
        {
            try
            {
                // تحقق من وجود سجل طبي للحيوان
                var medicalRecord = await unitOfWork.medicalRecordRepository.GetByAnimalIdAsync(animalId);
                if (medicalRecord == null)
                {
                    return Json(new { exists = false, message = "No medical record found for this animal." });
                }

                // إذا وجد سجل طبي، قم بإرجاع تفاصيل الحيوان والمزرعة
                var animal = await unitOfWork.animalRepository.GetAsync(animalId);
                if (animal == null)
                {
                    return Json(new { exists = false, message = "Animal not found." });
                }

                return Json(new
                {
                    exists = true,
                    animalName = animal.Name,
                    farmName = animal.Farm?.Name
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while checking animal medical record.");
                return Json(new { error = "An error occurred while checking animal medical record." });
            }
        }
    }
}