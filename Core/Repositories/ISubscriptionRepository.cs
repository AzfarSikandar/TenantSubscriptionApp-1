using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;

namespace TenantSubscriptionApp.Core.Repositories
{
    public interface ISubscriptionRepository
    {
        ICollection<TenantSubscription> GetSubscriptions(int organisationId);

        Task<bool> CheckDbCreated(TenantSubscription input);

        Task<bool> AddSubscription(int applicationId, ApplicationUser user);
    }
}
