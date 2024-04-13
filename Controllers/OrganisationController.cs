using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TenantSubscriptionApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TenantSubscriptionApp.Core;

namespace TenantSubscriptionApp.Controllers
{
    public class OrganisationController : Controller
    {
        IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrganisationController(IUnitOfWork unitOfWork, 
            UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult OrganisationIndex()
        {
            var organisations = _unitOfWork.Organisation.GetOrganisations();

            return View(organisations);
        }

        public async Task<IActionResult> Add()
        {
            return View();
        }

        [Authorize (Roles = $"{Constants.Roles.Administrator}")]
        public async Task<IActionResult> AddOrganisation(Organisation input)
        {
            try
            {
                await _unitOfWork.Organisation.InsertOrganisation(input);
                var user = new ApplicationUser();

                user.OrganisationId = input.Id;
                user.FirstName = $"Manger{(input.Name).Split(' ').FirstOrDefault()}";
                user.LastName = $"Default";
                var email = $"Manager@{(input.Name).Split(' ').FirstOrDefault()}.com";

                user.Email = email ;
                user.UserName = email;

                var result = await _userManager.CreateAsync(user, $"Admin@123");
                await _userManager.AddToRoleAsync(user, "Manager");
                await _userManager.AddToRoleAsync(user, "User");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return RedirectToAction("OrganisationIndex");
        }

        //public async Task<IActionResult> Edit(int id)
        //{

        //    var organisation = _unitOfWork.Organisation.
        //    return View()
        //}


    }
}
