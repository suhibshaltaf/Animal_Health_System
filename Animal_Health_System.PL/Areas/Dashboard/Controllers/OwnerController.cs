using Animal_Health_System.BLL.Interface;
using Animal_Health_System.BLL.Repository;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.OwnerVIMO;
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
    public class OwnerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<OwnerController> logger;

        public OwnerController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<OwnerController> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                var owners = await unitOfWork.ownerRepository.GetAllAsync();
                var ownerModels = mapper.Map<IEnumerable<OwnerVM>>(owners);
                return View(ownerModels);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while fetching owners.");
                TempData["ErrorMessage"] = "An error occurred while fetching the owners.";
                return View(new HashSet<OwnerVM>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OwnerFormVM ownerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please check the entered data.";
                    return View(ownerVM);
                }

                var owner = mapper.Map<Owner>(ownerVM);
                await unitOfWork.ownerRepository.AddAsync(owner);

                TempData["SuccessMessage"] = "Owner added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while creating the owner.");
                TempData["ErrorMessage"] = "An error occurred while creating the owner.";
                return View(ownerVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var owner = await unitOfWork.ownerRepository.GetAsync(id);
                if (owner == null)
                {
                    TempData["ErrorMessage"] = "Owner not found.";
                    return RedirectToAction("Index");
                }

                var ownerVM = mapper.Map<OwnerFormVM>(owner);
                return View(ownerVM);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred while fetching the owner with ID {id}.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OwnerFormVM ownerVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["ErrorMessage"] = "Please check the entered data.";
                    return View(ownerVM);
                }

                var owner = await unitOfWork.ownerRepository.GetAsync(ownerVM.Id);
                if (owner == null)
                {
                    TempData["ErrorMessage"] = "Owner not found.";
                    return RedirectToAction("Index");
                }

                // تحديث بيانات الـ Owner
                mapper.Map(ownerVM, owner);
                await unitOfWork.ownerRepository.UpdateAsync(owner);

                // تحديث المستخدم المرتبط بالـ Owner باستخدام UserManager
                var user = await unitOfWork.UserManager.FindByIdAsync(owner.ApplicationUserId.ToString());
                if (user != null)
                {
                    user.FullName = ownerVM.FullName; // تحديث الاسم الكامل في الـ User
                    var result = await unitOfWork.UserManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        TempData["ErrorMessage"] = "An error occurred while updating the user.";
                        return View(ownerVM);
                    }
                }

                TempData["SuccessMessage"] = "Owner and User updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the owner.");
                TempData["ErrorMessage"] = "An error occurred while updating the owner. Please try again later.";
                return View(ownerVM);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                // الحصول على المالك باستخدام الـ id
                var owner = await unitOfWork.ownerRepository.GetAsync(id);
                if (owner == null)
                {
                    return NotFound();
                }

                // تحويل الكائن من Owner إلى OwnerDetailsVM
                var ownerModel = mapper.Map<OwnerDetailsVM>(owner);

                // عرض تفاصيل المالك
                return View(ownerModel);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ في حال حدوث استثناء
                logger.LogError(ex, "Error occurred while fetching owner details.");

                // إظهار رسالة خطأ للمستخدم
                TempData["ErrorMessage"] = "An error occurred while fetching owner details. Please try again later.";

                // العودة إلى صفحة Index
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var owner = await unitOfWork.ownerRepository.GetAsync(id);
                if (owner == null)
                {
                    return NotFound();
                }

                await unitOfWork.ownerRepository.DeleteAsync(id);
                TempData["SuccessMessage"] = "owner deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while deleting owner.");
                TempData["ErrorMessage"] = "An error occurred while deleting the owner. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
