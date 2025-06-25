using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineHistoryVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class VaccineHistoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<VaccineHistoryController> logger;

        public VaccineHistoryController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VaccineHistoryController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        // GET: Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vaccineHistorys = await unitOfWork.vaccineHistoryRepository.GetAllAsync();
            var vaccineHistoryVm = mapper.Map<IEnumerable<VaccineHistoryVM>>(vaccineHistorys);
            return View(vaccineHistoryVm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var vaccineHistory = await unitOfWork.vaccineHistoryRepository.GetAsync(id);
            if (vaccineHistory == null)
                return NotFound();

            var viewModel = mapper.Map<VaccineHistoryDetailsVM>(vaccineHistory);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var vm = new VaccineHistoryFormVM
                {
                    Veterinarians = new SelectList(await unitOfWork.veterinarianRepository.GetAllAsync(), "Id", "FullName"),
                    Farms = new SelectList(await unitOfWork.farmRepository.GetAllAsync(), "Id", "Name"),
                    Vaccines = new SelectList(await unitOfWork.vaccineRepository.GetAllAsync(), "Id", "Name"),
                    Animals = new List<SelectListItem>(), // Initialize Animals as an empty list
                    MedicalRecords = new List<SelectListItem>() // Initialize MedicalRecords as an empty list
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
        public async Task<IActionResult> Create(VaccineHistoryFormVM vm)
        {
            if ( ModelState.IsValid)
            {
                // إعادة تهيئة القوائم المنسدلة بعد فشل التحقق من الصحة
                vm.Veterinarians = new SelectList(await unitOfWork.veterinarianRepository.GetAllAsync(), "Id", "FullName", vm.VeterinarianId);
                vm.Farms = new SelectList(await unitOfWork.farmRepository.GetAllAsync(), "Id", "Name", vm.FarmId);
                vm.Vaccines = new SelectList(await unitOfWork.vaccineRepository.GetAllAsync(), "Id", "Name", vm.VaccineId);
                TempData["Error"] = "Please fill all required fields.";
                return View(vm);
            }

            try
            {
                var vaccineHistory = mapper.Map<VaccineHistory>(vm);
                await unitOfWork.vaccineHistoryRepository.AddAsync(vaccineHistory);
                await unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Vaccine history added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating vaccine history.");
                // إعادة تهيئة القوائم المنسدلة في حال حدوث خطأ
                vm.Veterinarians = new SelectList(await unitOfWork.veterinarianRepository.GetAllAsync(), "Id", "FullName", vm.VeterinarianId);
                vm.Farms = new SelectList(await unitOfWork.farmRepository.GetAllAsync(), "Id", "Name", vm.FarmId);
                vm.Vaccines = new SelectList(await unitOfWork.vaccineRepository.GetAllAsync(), "Id", "Name", vm.VaccineId);
                TempData["Error"] = "An error occurred while saving the vaccine history. Please try again.";
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            logger.LogInformation($"Edit action called with ID: {id}");

            var vaccineHistory = await unitOfWork.vaccineHistoryRepository.GetAsync(id);
            if (vaccineHistory == null)
            {
                TempData["Error"] = "Vaccine history not found.";
                return RedirectToAction("Index");
            }

            var vm = mapper.Map<VaccineHistoryFormVM>(vaccineHistory);
            vm.Veterinarians = (await unitOfWork.veterinarianRepository.GetAllAsync())
                                    .Select(v => new SelectListItem { Text = v.FullName, Value = v.Id.ToString() })
                                    .ToList();
            vm.Farms = (await unitOfWork.farmRepository.GetAllAsync())
                           .Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() })
                           .ToList();
            vm.Vaccines = (await unitOfWork.vaccineRepository.GetAllAsync())
                                .Select(v => new SelectListItem { Text = v.Name, Value = v.Id.ToString() })
                                .ToList();
            vm.Animals = (await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(vm.FarmId ?? 0))
                                .Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() })
                                .ToList();
            vm.MedicalRecords = (await unitOfWork.medicalRecordRepository.GetByFarmAsync(vm.FarmId ?? 0))
                         .Select(mr => new SelectListItem { Text = mr.Name.ToString(), Value = mr.Id.ToString() })
                         .ToList();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VaccineHistoryFormVM vm)
        {
            if ( ModelState.IsValid)
            {
                vm.Veterinarians = (await unitOfWork.veterinarianRepository.GetAllAsync())
                                    .Select(v => new SelectListItem { Text = v.FullName, Value = v.Id.ToString() })
                                    .ToList();
                vm.Farms = (await unitOfWork.farmRepository.GetAllAsync())
                               .Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() })
                               .ToList();
                vm.Vaccines = (await unitOfWork.vaccineRepository.GetAllAsync())
                                    .Select(v => new SelectListItem { Text = v.Name, Value = v.Id.ToString() })
                                    .ToList();
                vm.Animals = (await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(vm.FarmId ?? 0))
                                    .Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() })
                                    .ToList();
                vm.MedicalRecords = (await unitOfWork.medicalRecordRepository.GetByFarmAsync(vm.FarmId ?? 0))
                                    .Select(mr => new SelectListItem { Text = mr.Name, Value = mr.Id.ToString() })
                                    .ToList();
                TempData["Error"] = "Please fill all required fields.";
                return View(vm);
            }

            try
            {
                var vaccineHistory = mapper.Map<VaccineHistory>(vm);
                await unitOfWork.vaccineHistoryRepository.UpdateAsync(vaccineHistory);
                await unitOfWork.SaveChangesAsync();

                TempData["Success"] = "Vaccine history updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error updating vaccine history with ID {vm.Id}.");
                vm.Veterinarians = (await unitOfWork.veterinarianRepository.GetAllAsync())
                                    .Select(v => new SelectListItem { Text = v.FullName, Value = v.Id.ToString() })
                                    .ToList();
                vm.Farms = (await unitOfWork.farmRepository.GetAllAsync())
                               .Select(f => new SelectListItem { Text = f.Name, Value = f.Id.ToString() })
                               .ToList();
                vm.Vaccines = (await unitOfWork.vaccineRepository.GetAllAsync())
                                    .Select(v => new SelectListItem { Text = v.Name, Value = v.Id.ToString() })
                                    .ToList();
                vm.Animals = (await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(vm.FarmId ?? 0))
                                    .Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() })
                                    .ToList();
                vm.MedicalRecords = (await unitOfWork.medicalRecordRepository.GetByFarmAsync(vm.FarmId ?? 0))
                     .Select(mr => new SelectListItem { Text = mr.Name, Value = mr.Id.ToString() })
                     .ToList();

                TempData["Error"] = "An error occurred while updating the vaccine history. Please try again.";
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vaccineHistory = await unitOfWork.vaccineHistoryRepository.GetAsync(id);
            if (vaccineHistory == null)
            {
                TempData["Error"] = "Vaccine History not found.";
                return RedirectToAction(nameof(Index));
            }

            if (vaccineHistory.IsDeleted)
            {
                return BadRequest("The Vaccine History is already deleted.");
            }

            await unitOfWork.vaccineHistoryRepository.DeleteAsync(id);
            await unitOfWork.SaveChangesAsync();

            TempData["Success"] = "Vaccine History deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> GetAnimalsByFarm(int farmId)
        {
            var animals = await unitOfWork.animalRepository.GetAnimalsByFarmIdAsync(farmId);
            Console.WriteLine("Animals: " + animals.Count());  // ضع هنا لتتأكد من أن البيانات تأتي
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
