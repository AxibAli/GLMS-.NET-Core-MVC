using GLMS.Constants;
using GLMS.Manager.Helpers;
using GLMS.Manager.Seeds;
using GLMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GLMS.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]

    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public PermissionController(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Add(int roleId)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Actions), roleId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            model.RoleId = roleId;
            var claims = await _roleManager.GetClaimsAsync(role);
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId.ToString());
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            //var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            //foreach (var claim in selectedClaims)
            //{
            //    await _roleManager.AddPermissionClaim(role, claim.Value);
            //}
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
