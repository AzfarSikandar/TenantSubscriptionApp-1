using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace TenantSubscriptionApp.Controllers
{
    public class OrganisationController : Controller
    {
        IUnitOfWork _unitOfWork;
        public OrganisationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public async Task<IActionResult> AddOrganisation(Organisation input)
        {
            try
            {
                await _unitOfWork.Organisation.InsertOrganisation(input);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

            return RedirectToAction("OrganisationIndex");
        }


    }
}
