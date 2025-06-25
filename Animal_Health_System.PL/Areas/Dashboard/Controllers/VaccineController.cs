using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.VaccineVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class VaccineController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<VaccineController> logger;

        public VaccineController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<VaccineController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var vaccines = await unitOfWork.vaccineRepository.GetAllAsync();
                var vaccineVm = mapper.Map<IEnumerable<VaccineVM>>(vaccines);
                return View(vaccineVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching vaccines.");
                TempData["ErrorMessage"] = "An error occurred while fetching vaccines.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VaccineFormVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await unitOfWork.vaccineRepository.ExistsByNameAsync(vm.Name))
                    {
                        ModelState.AddModelError("Name", "This vaccine already exists.");
                        return View(vm);
                    }

                    if (vm.ProductionDate < new DateTime(2025, 2, 10) || vm.ProductionDate > DateTime.UtcNow.Date)
            {
                        ModelState.AddModelError("ProductionDate", "Production date must be between 2025/2/10 and today's date.");
                    }

                    if (vm.ExpiryDate <= vm.ProductionDate)
                    {
                        ModelState.AddModelError("ExpiryDate", "Expiry date must be later than production date.");
                    }

                    if (vm.ExpiryDate > new DateTime(2025, 5, 1))
                    {
                        ModelState.AddModelError("ExpiryDate", "Expiry date cannot be later than 2025/5/1.");
                    }

                    // Map the ViewModel to the Entity
                    var vaccine = mapper.Map<Vaccine>(vm);
                    vm.UpdatedAt = DateTime.UtcNow;

                    // Add the vaccine to the database
                    await unitOfWork.vaccineRepository.AddAsync(vaccine);
                    await unitOfWork.vaccineRepository.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Vaccine created successfully.";
                    return RedirectToAction(nameof(Index));
                }

                // If ModelState is invalid, return the view with validation errors
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating vaccine.");
                TempData["ErrorMessage"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var vaccine = await unitOfWork.vaccineRepository.GetAsync(id);
                if (vaccine == null)
                {
                    return NotFound();
                }
                var vm = mapper.Map<VaccineFormVM>(vaccine);
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while editing vaccine.");
                TempData["ErrorMessage"] = "An error occurred while editing vaccine.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VaccineFormVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await unitOfWork.vaccineRepository.ExistsByNameAsync(vm.Name))
                    {
                        ModelState.AddModelError("Name", "This vaccine already exists.");
                        return View(vm);
                    }

                    if (vm.ProductionDate < new DateTime(2025, 2, 10) || vm.ProductionDate > DateTime.UtcNow.Date)
                    {
                        ModelState.AddModelError("ProductionDate", "Production date must be between 2025/2/10 and today's date.");
                    }

                    if (vm.ExpiryDate <= vm.ProductionDate)
                    {
                        ModelState.AddModelError("ExpiryDate", "Expiry date must be later than production date.");
                    }

                    if (vm.ExpiryDate > new DateTime(2025, 5, 1))
                    {
                        ModelState.AddModelError("ExpiryDate", "Expiry date cannot be later than 2025/5/1.");
                    }
                    var vaccine = await unitOfWork.vaccineRepository.GetAsync(vm.Id);
                    if (vaccine == null)
                    {
                        return NotFound();
                    }
                    mapper.Map(vm, vaccine);
                    vm.UpdatedAt = DateTime.UtcNow;
                    await unitOfWork.vaccineRepository.UpdateAsync(vaccine);
                    await unitOfWork.vaccineRepository.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Vaccine updated successfully.";
                    return RedirectToAction(nameof(Index));
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating vaccine.");
                TempData["ErrorMessage"] = ex.Message;
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var vaccine = await unitOfWork.vaccineRepository.GetAsync(id);
                if (vaccine == null)
                {
                    return NotFound();
                }
                var viewModel = mapper.Map<VaccineDetailsVM>(vaccine);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching vaccine details.");
                TempData["ErrorMessage"] = "An error occurred while fetching vaccine details.";
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await unitOfWork.vaccineRepository.DeleteAsync(id);
                await unitOfWork.vaccineRepository.SaveChangesAsync();
                return Ok(new { Message = "Vaccine deleted successfully" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting vaccine.");
                return StatusCode(500, new { Message = "An error occurred while deleting vaccine." });
            }
        }
    }
}