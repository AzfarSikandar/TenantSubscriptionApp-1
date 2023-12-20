using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using Microsoft.AspNetCore.Identity;

namespace TenantSubscriptionApp.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AuthDBContext _context;
        public RoleRepository(AuthDBContext context)
        {
            _context = context;
        }

        public ICollection<IdentityRole> GetRoles()
        {
            return _context.Roles.ToList();
        }
    }
}
