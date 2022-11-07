using GLMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;
        private readonly string? result;

        public AdminController(RoleManager<IdentityRole<int>> roleManager)
        {
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(ProjectRole role)
        {
            var roleExist = await roleManager.RoleExistsAsync(role.RoleName);
            if (!roleExist)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<int>(role.RoleName));
            }
            return View(result);
        }
    }
}
