using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Animal_Health_System.DAL.Data;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Animal_Health_System.BLL.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class AnimalController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<AnimalController> logger;

        public AnimalController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AnimalController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        // Get all animals
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var animals = await unitOfWork.animalRepository.GetAllAsync();
                var animalVm = mapper.Map<IEnumerable<AnimalVM>>(animals);
                return View(animalVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching animals.");
                TempData["ErrorMessage"] = "An error occurred while loading the animal data.";
                return RedirectToAction("Index");
            }
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                var vm = new AnimalFormVM
                {
                    Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while preparing the create view.");
                TempData["ErrorMessage"] = "An error occurred while preparing the form.";
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnimalFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(vm);
            }

            try
            {
                var animal = mapper.Map<Animal>(vm);
                await unitOfWork.animalRepository.AddAsync(animal);
                TempData["SuccessMessage"] = "Animal added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding the animal.");
                TempData["ErrorMessage"] = "An error occurred while adding the animal.";
                return View(vm);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var animal = await unitOfWork.animalRepository.GetAsync(id);
                if (animal == null)
                {
                    TempData["ErrorMessage"] = "Animal not found.";
                    return RedirectToAction(nameof(Index));
                }

                var farms = await unitOfWork.farmRepository.GetAllAsync();
                var vm = mapper.Map<AnimalFormVM>(animal);
                vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while preparing the edit view.");
                TempData["ErrorMessage"] = "An error occurred while preparing the form.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnimalFormVM vm)
        {
            if (!ModelState.IsValid)
            {
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                vm.Farms = farms.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                TempData["ErrorMessage"] = "Please correct the errors and try again.";
                return View(vm);
            }

            try
            {
                var animal = await unitOfWork.animalRepository.GetAsync(vm.Id);
                if (animal == null)
                {
                    TempData["ErrorMessage"] = "Animal not found.";
                    return RedirectToAction(nameof(Index));
                }

                mapper.Map(vm, animal);
                await unitOfWork.animalRepository.UpdateAsync(animal);
                TempData["SuccessMessage"] = "Animal updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the animal.");
                TempData["ErrorMessage"] = "An error occurred while updating the animal.";
                return View(vm);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var animal = await unitOfWork.animalRepository.GetAsync(id);
                if (animal == null)
                {
                    TempData["ErrorMessage"] = "Animal not found.";
                    return RedirectToAction(nameof(Index));
                }

                var animalVm = mapper.Map<AnimalDetailsVM>(animal);
                return View(animalVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving animal details.");
                TempData["ErrorMessage"] = "An error occurred while fetching animal details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Soft delete animal
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var animal = await unitOfWork.animalRepository.GetAsync(id);
                if (animal == null)
                {
                    return Json(new { success = false, message = "Animal not found." });
                }

                await unitOfWork.animalRepository.DeleteAsync(id);
                return Json(new { success = true, message = "Animal deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the animal.");
                return Json(new { success = false, message = "An error occurred while deleting the animal." });
            }
        }
    }
}
