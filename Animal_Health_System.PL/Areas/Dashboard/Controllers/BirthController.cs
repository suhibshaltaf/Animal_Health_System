using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.AnimalVIMO;
using Animal_Health_System.PL.Areas.Dashboard.ViewModels.BirthVIMO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Animal_Health_System.PL.Areas.Dashboard.Controllers
{
    [Authorize]

    [Area("Dashboard")]
    public class BirthController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<BirthController> logger;

        public BirthController(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BirthController> logger)
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
                var births = await unitOfWork.birthRepository.GetAllAsync(
                    includeProperties: "Pregnancy"  // تحميل بيانات الحمل المرتبطة
                );

                var birthVm = mapper.Map<IEnumerable<BirthVM>>(births);
                return View(birthVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while fetching births.");
                TempData["ErrorMessage"] = "An error occurred while loading the birth data.";
                return RedirectToAction(nameof(Index));
            }
        }





        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                var animals = await unitOfWork.animalRepository.GetAllAsync();  // إضافة جلب الحيوانات

                var vm = new BirthFormVM
                {
                    Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList(),
                    Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList()  // إضافة قائمة الحيوانات
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
        public async Task<IActionResult> Create(BirthFormVM vm)
        {
            try
            {
                // تحديد التاريخ الذي لا يمكن إضافة الولادة قبله أو بعده
                DateTime startDate = vm.BirthDate.AddMonths(-2);  // قبل شهرين
                DateTime endDate = vm.BirthDate.AddMonths(2);      // بعد شهرين

                // التحقق من وجود ولادة بنفس الـ PregnancyId في نفس الفترة الزمنية
                var existingBirths = await unitOfWork.birthRepository.GetAllAsync();
                var isPregnancyExist = existingBirths.Any(b => b.PregnancyId == vm.PregnancyId && b.BirthDate >= startDate && b.BirthDate <= endDate);

                if (isPregnancyExist)
                {
                    TempData["ErrorMessage"] = "A birth with this pregnancy already exists within the specified date range (2 months before or after).";
                    var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                    var animals = await unitOfWork.animalRepository.GetAllAsync();
                    vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                    vm.Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
                    return View(vm);
                }

                if (!ModelState.IsValid)
                {
                    var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                    var animals = await unitOfWork.animalRepository.GetAllAsync();
                    vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                    vm.Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return View(vm);
                }

                var birth = mapper.Map<Birth>(vm);
                await unitOfWork.birthRepository.AddAsync(birth);
                TempData["SuccessMessage"] = "Birth added successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while adding the birth.");
                TempData["ErrorMessage"] = "An error occurred while adding the birth.";

                var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                var animals = await unitOfWork.animalRepository.GetAllAsync();
                vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                vm.Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
                return View(vm);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // استرجاع بيانات الولادة بناءً على الـ id
                var birth = await unitOfWork.birthRepository.GetAsync(id);
                if (birth == null)
                {
                    TempData["ErrorMessage"] = "Birth not found.";
                    return RedirectToAction(nameof(Index));
                }

                // استرجاع قائمة الحمل والأنواع
                var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                var animals = await unitOfWork.animalRepository.GetAllAsync();
                if (pregnancies == null || animals == null)
                {
                    logger.LogError("Failed to retrieve pregnancies or animals.");
                    TempData["ErrorMessage"] = "Failed to load data.";
                    return RedirectToAction(nameof(Index));
                }

                // تعيين الـ ViewModel باستخدام AutoMapper
                var vm = mapper.Map<BirthFormVM>(birth);

                // تعيين الـ Pregnancy و Animal يدوياً
                vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                vm.Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();

                return View(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while preparing the edit view.");
                TempData["ErrorMessage"] = $"An error occurred while preparing the form: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BirthFormVM vm)
        {
            try
            {
                // تحديد التاريخ الذي لا يمكن إضافة الولادة قبله أو بعده
                DateTime startDate = vm.BirthDate.AddMonths(-2);  // قبل شهرين
                DateTime endDate = vm.BirthDate.AddMonths(2);      // بعد شهرين

                // التحقق من وجود ولادة بنفس الـ PregnancyId في نفس الفترة الزمنية إلا إذا كانت الولادة الحالية هي نفس الولادة
                var existingBirths = await unitOfWork.birthRepository.GetAllAsync();
                var isPregnancyExist = existingBirths.Any(b => b.PregnancyId == vm.PregnancyId && b.BirthDate >= startDate && b.BirthDate <= endDate && b.Id != vm.Id);

                if (isPregnancyExist)
                {
                    TempData["ErrorMessage"] = "A birth with this pregnancy already exists within the specified date range (2 months before or after).";
                    var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                    var animals = await unitOfWork.animalRepository.GetAllAsync();
                    vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                    vm.Animal = animals.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Name }).ToList();
                    return View(vm);
                }

                // التحقق من صحة النموذج
                if (!ModelState.IsValid)
                {
                    // استرجاع قائمة الحمل والأنواع لعرضها في حالة وجود أخطاء
                    var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                    vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();

                    TempData["ErrorMessage"] = "Please correct the errors and try again.";
                    return View(vm);
                }

                // استرجاع بيانات الولادة بناءً على الـ id
                var birth = await unitOfWork.birthRepository.GetAsync(vm.Id);
                if (birth == null)
                {
                    TempData["ErrorMessage"] = "Birth not found.";
                    return RedirectToAction(nameof(Index));
                }

                // تعيين البيانات من الـ ViewModel إلى كائن Birth
                mapper.Map(vm, birth);
                await unitOfWork.birthRepository.UpdateAsync(birth);

                TempData["SuccessMessage"] = "Birth updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while updating the birth.");
                TempData["ErrorMessage"] = $"An error occurred while updating the birth: {ex.Message}";

                // استرجاع قائمة الحمل لعرضها في حالة حدوث خطأ
                var pregnancies = await unitOfWork.pregnancyRepository.GetAllAsync();
                vm.Pregnancy = pregnancies.Select(f => new SelectListItem { Value = f.Id.ToString(), Text = f.Name }).ToList();
                return View(vm);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var birth = await unitOfWork.birthRepository.GetAsync(id, "Pregnancy");

                if (birth == null)
                {
                    TempData["ErrorMessage"] = "Birth record not found.";
                    return RedirectToAction(nameof(Index));
                }

                var birthDetailsVm = mapper.Map<BirthDetailsVM>(birth);
                return View(birthDetailsVm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while retrieving birth details.");
                TempData["ErrorMessage"] = "An error occurred while fetching birth details.";
                return RedirectToAction(nameof(Index));
            }
        }


        // Soft delete birth
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var birth = await unitOfWork.birthRepository.GetAsync(id);
                if (birth == null)
                {
                    return Json(new { success = false, message = "Birth not found." });
                }

                await unitOfWork.birthRepository.DeleteAsync(id);
                return Json(new { success = true, message = "Birth deleted successfully." });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the birth.");
                return Json(new { success = false, message = "An error occurred while deleting the birth." });
            }
        }
    }
}
