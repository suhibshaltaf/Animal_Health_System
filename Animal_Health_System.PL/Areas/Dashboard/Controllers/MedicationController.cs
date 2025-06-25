using Animal_Health_System.BLL.Interface;
using Animal_Health_System.BLL.Repository;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.MedicationVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VeterinarianVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]

    public class MedicationController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<MedicationController> logger;

        public MedicationController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicationController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var medications = await unitOfWork.medicationRepository.GetAllAsync();
            var medicationVMs = mapper.Map<IEnumerable<MedicationVM>>(medications);
            return View(medicationVMs);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MedicationFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // فحص تواريخ الإنتاج والانتهاء
            if (vm.ProductionDate < new DateTime(2022, 1, 1) || vm.ProductionDate > DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("ProductionDate", "Production date must be between 2022 and today's date.");
            }

            if (vm.ExpiryDate <= vm.ProductionDate)
            {
                ModelState.AddModelError("ExpiryDate", "Expiry date must be later than production date.");
            }

            if (vm.ExpiryDate > new DateTime(2030, 12, 31))
            {
                ModelState.AddModelError("ExpiryDate", "Expiry date cannot be later than December 31, 2030.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var medication = mapper.Map<Medication>(vm);
                await unitOfWork.medicationRepository.AddAsync(medication);
                await unitOfWork.medicationRepository.SaveChangesAsync();
                TempData["Success"] = "Medication added successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding medication.");

                if (ex.Message.Contains("already exists"))
                {
                    ModelState.AddModelError("Name", "A medication with the same name already exists.");
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var medication = await unitOfWork.medicationRepository.GetAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            var medicationVM = mapper.Map<MedicationFormVM>(medication);
            return View(medicationVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MedicationFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            // فحص تواريخ الإنتاج والانتهاء
            if (vm.ProductionDate < new DateTime(2022, 1, 1) || vm.ProductionDate > DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("ProductionDate", "Production date must be between 2022 and today's date.");
            }

            if (vm.ExpiryDate <= vm.ProductionDate)
            {
                ModelState.AddModelError("ExpiryDate", "Expiry date must be later than production date.");
            }

            if (vm.ExpiryDate > new DateTime(2030, 12, 31))
            {
                ModelState.AddModelError("ExpiryDate", "Expiry date cannot be later than December 31, 2030.");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            try
            {
                var medication = await unitOfWork.medicationRepository.GetAsync(vm.Id);
                if (medication == null)
                {
                    return NotFound();
                }

                mapper.Map(vm, medication);
                await unitOfWork.medicationRepository.UpdateAsync(medication);
                await unitOfWork.medicationRepository.SaveChangesAsync();
                TempData["Success"] = "Medication updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating medication.");

                if (ex.Message.Contains("already exists"))
                {
                    ModelState.AddModelError("Name", "A medication with the same name already exists.");
                }
                else
                {
                    ModelState.AddModelError("", ex.Message);
                }

                return View(vm);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var medication = await unitOfWork.medicationRepository.GetAsync(id);
            if (medication == null)
            {
                return NotFound();
            }

            var medicationVM = mapper.Map<MedicationDetailsVM>(medication);
            return View(medicationVM);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await unitOfWork.medicationRepository.DeleteAsync(id);
            await unitOfWork.medicationRepository.SaveChangesAsync();
            return Ok(new { Message = "Medication deleted" });
        }
    }
}
