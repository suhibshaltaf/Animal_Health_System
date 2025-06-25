using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.FarmStaffVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class FarmStaffController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<FarmStaffController> logger;

        public FarmStaffController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FarmStaffController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        // Get all FarmStaff
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var farmStaffHashSet = await unitOfWork.farmStaffRepository.GetAllAsync();
                var farmStaffVm = mapper.Map<IEnumerable<FarmStaffVM>>(farmStaffHashSet);
                return View(farmStaffVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching farm staff.");
                TempData["ErrorMessage"] = "An error occurred while loading the farm staff data.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var farms = await unitOfWork.farmRepository.GetAllAsync();
                var vm = new FarmStaffFormVM
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
        public async Task<IActionResult> Create(FarmStaffFormVM vm)
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
                var farmStaff = mapper.Map<FarmStaff>(vm);
                await unitOfWork.farmStaffRepository.AddAsync(farmStaff);
                TempData["SuccessMessage"] = "farmstaff added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding the farmStaff.");
                TempData["ErrorMessage"] = "An error occurred while adding the farmStaff.";
                return View(vm);
            }
        }

        // Edit GET action
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var farmStaff = await unitOfWork.farmStaffRepository.GetAsync(id);
                if (farmStaff == null)
                {
                    TempData["ErrorMessage"] = "farmStaff not found.";
                    return RedirectToAction(nameof(Index));
                }

                var farms = await unitOfWork.farmRepository.GetAllAsync();
                var vm = mapper.Map<FarmStaffFormVM>(farmStaff);
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

        // Edit POST action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FarmStaffFormVM vm)
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
                var farmStaff = await unitOfWork.farmStaffRepository.GetAsync(vm.Id);
                if (farmStaff == null)
                {
                    TempData["ErrorMessage"] = "Animal not found.";
                    return RedirectToAction(nameof(Index));
                }

                mapper.Map(vm, farmStaff);
                await unitOfWork.farmStaffRepository.UpdateAsync(farmStaff);
                TempData["SuccessMessage"] = "farmStaff updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the farmStaff.");
                TempData["ErrorMessage"] = "An error occurred while updating the farmStaff.";
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var farmstaff = await unitOfWork.farmStaffRepository.GetAsync(id);
                if (farmstaff == null)
                {
                    TempData["ErrorMessage"] = "farmstaff not found.";
                    return RedirectToAction(nameof(Index));
                }

                var farmstaffVm = mapper.Map<FarmStaffDetailsVM>(farmstaff);
                return View(farmstaffVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving farmstaff details.");
                TempData["ErrorMessage"] = "An error occurred while fetching farmstaff details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Soft delete farmstaff
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var farmstaff = await unitOfWork.farmStaffRepository.GetAsync(id);
                if (farmstaff == null)
                {
                    return Json(new { success = false, message = "farmstaff not found." });
                }

                await unitOfWork.farmStaffRepository.DeleteAsync(id);
                return Json(new { success = true, message = "farmstaff deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the farmstaff.");
                return Json(new { success = false, message = "An error occurred while deleting the farmstaff." });
            }
        }
    }
}
