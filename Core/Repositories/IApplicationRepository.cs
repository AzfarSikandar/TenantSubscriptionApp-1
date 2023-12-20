using TenantSubscriptionApp.Models;

namespace TenantSubscriptionApp.Core.Repositories
{
    public interface IApplicationRepository
    {
        ICollection<Application> GetApplications(); 
    }
}
