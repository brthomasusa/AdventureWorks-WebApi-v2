using Microsoft.AspNetCore.Mvc;

namespace AdventureWorks.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}