using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KyivBarGuideDomain.Model;
using KyivBarGuideInfrastructure;

public class ClientsController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly KyivBarGuideContext _context;

    public ClientsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, KyivBarGuideContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string email, string password, string name, string phoneNumber)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                PhoneNumber = phoneNumber,
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var client = new Client
                {
                    Name = name,
                    UserId = user.Id
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (ModelState.IsValid)
        {
            // Знайти користувача за email
            var user = await _userManager.FindByEmailAsync(email);

            // Перевірка на null, якщо користувача не знайдено
            if (user != null)
            {
                // Спроба увійти з користувачем за його UserName
                var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (result.Succeeded)
                {
                    // Успішний вхід
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Якщо спроба входу не вдалася
                    ModelState.AddModelError("", "Неправильний email або пароль.");
                }
            }
            else
            {
                // Користувач не знайдений
                ModelState.AddModelError("", "Користувача з таким email не знайдено.");
            }
        }

        // Якщо не вдалося увійти, повертаємо користувача назад до сторінки входу
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
