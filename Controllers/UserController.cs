using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace TenantSubscriptionApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserController(IUnitOfWork unitOfWork, SignInManager<ApplicationUser> signInManager)
        {
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> UserIndex()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var signedInUser = _unitOfWork.User.GetUser(userId);

            var users = _unitOfWork.User.GetUsers().ToList();

            if (!User.IsInRole("Administrator") )
            {
                users = users.Where(us => us.Organisation.Id == signedInUser.Organisation.Id).ToList();
            }

            return View(users);
        }


        [Authorize(Policy = $"{Constants.Policies.RequireAdminOrManager}")]
        public async Task<IActionResult> Edit(string id)
        {
            var user = _unitOfWork.User.GetUser(id);
            var roles = _unitOfWork.Role.GetRoles();
            
            var userRoles = await _signInManager.UserManager.GetRolesAsync(user);

            var roleItems = roles.Select(role =>
                new SelectListItem(
                    role.Name,
                    role.Id,
                    userRoles.Any(ur => ur.Contains(role.Name)), false)).ToList();


            if (userRoles.Contains("Manager") || userRoles.Contains("User"))
            {
                roleItems.ForEach(ur => ur.Disabled = ur.Text == "Administrator" ?  true : false );
            }

            var vm = new EditUserViewModel
            {
                User = user,
                Roles = roleItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(EditUserViewModel data)
        {
            var user = _unitOfWork.User.GetUser(data.User.Id);
            if (user == null)
            {
                return NotFound();
            }

            var userRolesInDb = await _signInManager.UserManager.GetRolesAsync(user);

            var rolesToAdd = new List<string>();
            var rolesToDelete = new List<string>();

            foreach (var role in data.Roles)
            {
                var assignedInDb = userRolesInDb.FirstOrDefault(ur => ur == role.Text);
                if (role.Selected)
                {
                    if (assignedInDb == null)
                    {
                        rolesToAdd.Add(role.Text);
                    }
                }
                else
                {
                    if (assignedInDb != null)
                    {
                        rolesToDelete.Add(role.Text);
                    }
                }
            }

            if (rolesToAdd.Any())
            {
                await _signInManager.UserManager.AddToRolesAsync(user, rolesToAdd);
            }

            if (rolesToDelete.Any())
            {
                await _signInManager.UserManager.RemoveFromRolesAsync(user, rolesToDelete);
            }
           

            user.FirstName = data.User.FirstName;
            user.LastName = data.User.LastName;
            user.Email = data.User.Email;

            _unitOfWork.User.UpdateUser(user);

            return RedirectToAction("UserIndex");
        }
    }
}
