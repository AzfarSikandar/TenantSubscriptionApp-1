using TenantSubscriptionApp.Core.Repositories;

namespace TenantSubscriptionApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; }
        public IRoleRepository Role { get; }
        public IOrganisationRepository Organisation { get; }
        public ISubscriptionRepository Subscription { get; }
        public IApplicationRepository Application { get; }

        public UnitOfWork(IUserRepository user,
            IRoleRepository role,
            IOrganisationRepository organisation,
            ISubscriptionRepository subscription,
            IApplicationRepository application)
        {
            User = user;
            Role = role;
            Organisation = organisation;
            Subscription = subscription;
            Application = application;
        }
    }
}
