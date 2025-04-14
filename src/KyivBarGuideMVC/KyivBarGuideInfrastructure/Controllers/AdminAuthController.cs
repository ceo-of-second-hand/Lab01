using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KyivBarGuideDomain.Model;
using System.Threading.Tasks;
using System.Security.Claims;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KyivBarGuideInfrastructure.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly KyivBarGuideContext _context;

        public AdminAuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            KyivBarGuideContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult SignUpAsAdmin()
        {
            return View(new AdminSignUpViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsAdmin(AdminSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user already has an admin profile
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    var existingAdmin = await _context.Admins
                        .FirstOrDefaultAsync(a => a.UserId == existingUser.Id);
                    if (existingAdmin != null)
                    {
                        ModelState.AddModelError("", "You are already registered as an admin");
                        return View(model);
                    }
                }

                // Create new bar
                var bar = new Bar
                {
                    Name = model.BarName,
                    Theme = model.BarTheme,
                    BarPassword = model.Password // Using the same password for both bar and admin
                };
                _context.Bars.Add(bar);
                await _context.SaveChangesAsync();

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create admin profile
                    var admin = new Admin
                    {
                        Name = model.Name,
                        UserId = user.Id,
                        WorkIn = bar
                    };

                    _context.Admins.Add(admin);
                    await _context.SaveChangesAsync();

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
    }

    public class AdminSignUpViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BarName { get; set; }

        [Required]
        public string BarTheme { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long")]
        public string Password { get; set; }
    }

    public class AdminLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "Password must be at least 4 characters long")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
} 