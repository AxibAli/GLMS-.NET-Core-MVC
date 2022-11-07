using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class ActionsController : Controller
    {

        public async Task<IActionResult> Action()
        {
            return View();
        }
    }
}
