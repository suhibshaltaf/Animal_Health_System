using Animal_Health_System.BLL.Interface;
using Animal_Health_System.DAL.Models;
    using Animal_Health_System.PL.Helpers;
    using Animal_Health_System.PL.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
    using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Security.Claims;

    namespace Animal_Health_System.PL.Controllers
    {
        public class AccountController : Controller
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUnitOfWork unitOfWork;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,IUnitOfWork unitOfWork)
            {
                this.userManager = userManager;
                this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Register()
        {
            LoadRolesIntoViewBag(); 
            return View();
        }

        private void LoadRolesIntoViewBag()
        {
            ViewBag.RolesList = new List<SelectListItem>
    {
        new SelectListItem { Value = "Owner", Text = "Owner" },
        new SelectListItem { Value = "FarmStaff", Text = "Farm Staff" },
        new SelectListItem { Value = "Veterinarian", Text = "Veterinarian" }
    };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                LoadRolesIntoViewBag();
                return View(model);
            }

            if (string.IsNullOrEmpty(model.SelectedRole))
            {
                ModelState.AddModelError("SelectedRole", "You must select a role.");
                LoadRolesIntoViewBag(); 
                return View(model);
            }

            var user = new ApplicationUser
            {
                FullName = model.FullName,
                Email = model.Email,
                UserName = model.Email,
                Address = model.Address,
                Role=model.SelectedRole
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, model.SelectedRole);
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, model.SelectedRole));

                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmEmailLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, protocol: HttpContext.Request.Scheme);
                var email = new DAL.Models.Email
                {
                    Subject = "Please Confirm Your Email",
                    Recivers = model.Email,
                    Body = $"Please confirm your account by clicking this link: {confirmEmailLink}"
                };

                try
                {
                    EmailSettings.SendEmail(email);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error sending confirmation email: {ex.Message}");
                    LoadRolesIntoViewBag();
                    return View(model);
                }

                return RedirectToAction(nameof(emailconf));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            LoadRolesIntoViewBag();
            return View(model);
        }







        public IActionResult emailconf()
            {
                return View();
            }
        [HttpGet]
        public async Task<IActionResult> confirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles.Count == 0)
                    {
                        await userManager.DeleteAsync(user);
                        ModelState.AddModelError("", "No role assigned. Your account has been deleted. Please register again and select a role.");
                        return RedirectToAction(nameof(Register));
                    }

                    if (!string.IsNullOrEmpty(user.Role)) 
                    {
                        var roleExists = await roleManager.RoleExistsAsync(user.Role);
                        if (!roleExists)
                        {
                            await roleManager.CreateAsync(new IdentityRole(user.Role));
                        }

                        await userManager.AddToRoleAsync(user, user.Role);
                    }

                    if (user.Role == "Veterinarian")
                    {
                        var veterinarian = new Veterinarian
                        {
                            FullName = user.FullName,
                            Specialty="def",
                            PhoneNumber ="0000000000",
                            Email = user.Email,
                            ApplicationUserId = user.Id,
                            YearsOfExperience = 0,
                            salary = 0
                        };

                        await unitOfWork.veterinarianRepository.AddAsync(veterinarian);
                        await unitOfWork.SaveChangesAsync();
                    }
                    else if (user.Role == "Owner")
                    {
                        var owner = new Owner
                        {
                            FullName = user.FullName,
                            PhoneNumber = "0000000000",
                            Email = user.Email,
                            ApplicationUserId = user.Id
                        };

                        await unitOfWork.ownerRepository.AddAsync(owner);
                        await unitOfWork.SaveChangesAsync();
                    }
                    else if (user.Role == "FarmStaff")
                    {
                       
                            var farmStaff = new FarmStaff
                            {
                                FullName = user.FullName,
                                JobTitle = "Default Job Title",
                                PhoneNumber = "0000000000",
                                Email = user.Email,
                                FarmId =1,
                                ApplicationUserId = user.Id,
                                DateHired = DateTime.Now,
                                salary=0

                            };

                            await unitOfWork.farmStaffRepository.AddAsync(farmStaff);
                            await unitOfWork.SaveChangesAsync();
                       
                    }

                    return RedirectToAction(nameof(Login));
                }
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpGet]
        public IActionResult Login()
            {
                return View();
            }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var check = await userManager.CheckPasswordAsync(user, model.Password);
                    if (check)
                    {
                        var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                        {
                            if (user.Role == "Admin")
                            {
                                return RedirectToAction("Index", "Animal", new { area = "Dashboard" });
                            }
                            else if (user.Role == "Owner")
                            {
                                return RedirectToAction("Index", "Owner", new { area = "Dashboard" });
                            }
                            else if (user.Role == "Veterinarian")
                            {
                                return RedirectToAction("Index", "Veterinarian", new { area = "Dashboard" });
                            }
                            else if (user.Role == "FarmStaff")
                            {
                                return RedirectToAction("Index", "FarmStaff", new { area = "Dashboard" });
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Incorrect password. Please try again.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No user found with this email. Please check your email and try again.");
                }
            }
            return View(model);
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordUrl(ForgotPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", model); 
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "No account found with this email.");
                return View("ForgotPassword", model);
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { email = model.Email, token = token }, protocol: HttpContext.Request.Scheme);

            var email = new DAL.Models.Email()
            {
                Subject = "Reset Password",
                Recivers = model.Email,
                Body = $"Click the link to reset your password: {resetPasswordUrl}",
            };

            EmailSettings.SendEmail(email);

           

            TempData["SuccessMessage"] = "Password reset link has been sent to your email.";
            return RedirectToAction(nameof(ForgotPassword));
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View(new ResetPasswordVM { Email = email, Token = token });
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request or user not found.");
                return View(model);
            }

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Password has been reset successfully.";
                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


    }
}
