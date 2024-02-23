using DotNetDemo.Models.DTO;
using DotNetDemo.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetDemo.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthentication _authService;
        public UserAuthenticationController(IUserAuthentication authService)
        {
            this._authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if (result.StatusCode == 1)
            {
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Registration(Registration model)
        {
            if (!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await this._authService.RegistrationAsync(model);
            TempData["msg"] = result.Message;
            if (result.StatusCode == 1)
            {
                return View("Login");
            }
            
            return RedirectToAction(nameof(Registration));
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        //Post Method forgot password
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            //if (!ModelState.IsValid) { return View(model); }
            var result=await this._authService.ForgotPasswordAsync(model);
            if (result.StatusCode == 1)
            {
                TempData["msg"] = result.Message;
                return View("Login");
            }
            return View();

        }

    }
}
