using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ActionsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
