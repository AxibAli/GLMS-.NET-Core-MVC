using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class LogView : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
