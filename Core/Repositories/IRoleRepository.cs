using Microsoft.AspNetCore.Identity;

namespace TenantSubscriptionApp.Core.Repositories
{
    public interface IRoleRepository
    {
        ICollection<IdentityRole> GetRoles();
    }
}
