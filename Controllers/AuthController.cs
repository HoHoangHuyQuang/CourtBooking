using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CourtBooking.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CourtBooking.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using CourtBooking.Utils;
using CourtBooking.ViewModels;

namespace CourtBooking.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _uow;

        public AuthController(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(loginRequest);
            }
            var obj = await _uow.AuthRepo.LoginValidate(loginRequest.Username, loginRequest.Password);
            if (obj != null)
            {

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, obj.UserName),
                        new Claim(ClaimTypes.Role, obj.Role.Name),
                      
                    };
                var claimsIdentity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme
                    );
                var principal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                    IsPersistent = loginRequest.RememberMe,
                    IssuedUtc = DateTime.Now,

                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    principal, authProperties);

                if (string.IsNullOrEmpty(loginRequest.ReturnUrl))
                {
                    return LocalRedirect("/");
                }
                return Redirect(loginRequest.ReturnUrl);

            }
            else
            {
                ModelState.AddModelError("Error", "Invalid username or password");

            }
            return View(loginRequest);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {            
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
     


        [AllowAnonymous]
        public async Task<IActionResult> Register(User user, string ConfirmPassword)
        {
            if (!ConfirmPassword.Equals(user.Password))
            {
                ModelState.AddModelError("", "Mật khẩu không trùng khớp!");
                ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
                return View(user);
            }
            if (await _uow.UserRepo.GetByUsername(user.UserName) != null)
            {
                ModelState.AddModelError("", "Username đã được sử dụng!");
                ViewData["RoleId"] = new SelectList(await _uow.RoleRepo.GetAll(), "Id", "Name", user.RoleId);
                return View(user);
            }
            user.Id = Guid.NewGuid();
            user.Password = AppUtils.HmacSha256Encrypt(user.Password);
            await _uow.UserRepo.Add(user);
            await _uow.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
