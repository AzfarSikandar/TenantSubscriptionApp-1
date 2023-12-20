namespace TenantSubscriptionApp.Core.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }

        IRoleRepository Role { get; }

        IOrganisationRepository Organisation { get; }

        ISubscriptionRepository Subscription { get; }

        IApplicationRepository Application { get; } 
    }
}
