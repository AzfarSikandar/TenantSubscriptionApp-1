using TenantSubscriptionApp.Core.ViewModels;

namespace TenantSubscriptionApp.Country
{
    public interface ICountryService
    {
        Task<List<CountryViewModel>> GetAllCountries(string userId);

        Task<CountryViewModel> GetCountry(int countryId);   

        Task<int> AddCountry(CountryInputModel country);

        Task<int> DeleteCountry(int id);

        Task<int> EditCountry(CountryViewModel country);
    }
}
