using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Country;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TenantSubscriptionApp.Controllers
{
    [Authorize]
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<CountryViewModel> countries = new();
            countries = await _countryService.GetAllCountries(userId);
            return View(countries);
        }

        //[HttpPost("Add")]
        public async Task<IActionResult> Add(CountryInputModel country)
        {
            // Handle the form submission, add the new country to the database
            if (ModelState.IsValid)
            {
                _countryService.AddCountry(country);
                // Redirect to the country index page after successful addition
                return RedirectToAction("Index");
            }

            // If there are validation errors, return to the form with errors
            return View(country);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            int response = 0;
            if (Id > 0)
            {
                response = await _countryService.DeleteCountry(Id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateCountry(CountryViewModel input)
        {
            int response = 0;

            if (ModelState.IsValid)
            {
                response = await _countryService.EditCountry(input);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetCountryById(int id)
        {
            // Retrieve the existing values for the country with the given id
            var country = await _countryService.GetCountry(id);// Retrieve the country from the database using id

            // Assuming you have an EditForm view for editing the country
            return PartialView("_EditForm", country);
        }


    }
}
