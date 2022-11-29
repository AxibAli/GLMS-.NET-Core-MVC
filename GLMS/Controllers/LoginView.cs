using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class LoginView : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
