using ExperimentalTask.Models.DTO;
using ExperimentalTask.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExperimentalTask.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _service;
        public UserAuthenticationController(IUserAuthenticationService service)
        {
            this._service = service;           
        }
        public IActionResult Registration()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.Role = "user";
            var result = await _service.RegisterAsync(model);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        { 
            if (!ModelState.IsValid)
	        {
                 return View(model);
	        }
            var result = await _service.LoginAsync(model);
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
            await _service.LogoutAsync();
            return RedirectToAction(nameof(Login));
        }

        //public async Task<IActionResult> RegisterAdmim()
        //{
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username = "admin",
        //        Email = "admin@gmail.com",
        //        FirstName = "parham",
        //        LastName = "mahmoudnia",
        //        Password = "Admin@12345#"
        //    };
        //    model.Role = "admin";
        //    var result = await this._service.RegisterAsync(model);
        //    return Ok(result);
        //}

    }
}
