using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuantumCloud.Data;
using QuantumCloud.Models;
using QuantumCloud.ViewModels;

namespace QuantumCloud.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(ApplicationDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Index(bool register = false)
        {
            ViewBag.Register = register;

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Email, Password, false, false);
                if (!result.Succeeded) return BadRequest("Error login gagal");

                return RedirectToAction("Index", "Home");

            }
            else
            {
                return BadRequest(ModelState.ErrorCount);
            }
        }
        public async Task<IActionResult> AuthUser(AuthVM model)
        {
            try
            {
                if (model.UserId != null)
                {
                    return View(model);
                }
                else
                {
                    var user = new User
                    {
                        Nama = model.Nama,
                        Email = model.Email,
                        UserName = model.Email,
                        NormalizedEmail = model.Email,
                        NormalizedUserName = model.Email,
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest("Registrasi gagal");
                    }
                }
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }
    }
}
