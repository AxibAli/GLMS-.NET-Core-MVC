using GLMS.Constants;
using GLMS.Manager.Helpers;
using GLMS.Manager.Seeds;
using GLMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index(int roleId)
        {
            var model = new Permission();
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

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Permission model)
        {
            var roleId = model.RoleId;
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Permission model)
        {
            var roleId = model.RoleId;
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            return View(model);
        }

        //public async Task<IActionResult> Edit(int roleId, [Bind("RoleId,RoleClaims")] PermissionViewModel model)
        //{
        //    if (roleId != model.RoleId)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _roleManager.Update(model);
        //            await _roleManager.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PermissionViewModelExists(model.RoleId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return View(model);
        //}
        //[HttpDelete, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int roleId)
        //{
        //    if (_roleManager.AddPermissionClaim == null)
        //    {
        //        return Problem("Error Null");
        //    }
        //    var permission = await _roleManager.FindByIdAsync(roleId.ToString());
        //    if (permission != null)
        //    {
        //        await _roleManager.RemoveClaimAsync( permission);
        //    }
        //    await _roleManager.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
        //private bool PermissionViewModelExists(int roleId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
