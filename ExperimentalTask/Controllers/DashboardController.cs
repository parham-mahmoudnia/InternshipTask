using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExperimentalTask.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        [Authorize]
        public IActionResult Display()
        {
            return View();
        }
    }
}
