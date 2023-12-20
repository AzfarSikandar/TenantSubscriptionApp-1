using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;

namespace TenantSubscriptionApp.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AuthDBContext _dbContext;
        public ApplicationRepository(AuthDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public ICollection<Application> GetApplications()
        {
            return _dbContext.Applications.ToList();
        }
    }
}
