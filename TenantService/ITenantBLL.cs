namespace TenantSubscriptionApp.TenantService
{
    public interface ITenantBLL
    {
        Task<string> CreateConnectionString(string connectionString);

        Task<string> GetTenantConnectionStringByUserId(string userId, string appName);
    }
}
