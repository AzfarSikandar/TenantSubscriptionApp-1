using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Identity;

namespace TenantSubscriptionApp.Core.Repositories
{
    public interface IOrganisationRepository
    {
        ICollection<Organisation> GetOrganisations();

        Task<bool> InsertOrganisation(Organisation input);

        //Task<bool> Update(int id);
    }
}
