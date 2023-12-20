using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using Microsoft.EntityFrameworkCore;

namespace TenantSubscriptionApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDBContext _context;

        public UserRepository(AuthDBContext context)
        {
            _context = context;
        }

        public ICollection<ApplicationUser> GetUsers()
        {

            return _context.Users.Include(us => us.Organisation).ToList();
        }

        public ApplicationUser GetUser(string id)
        {
            return _context.Users.Include(us => us.Organisation).FirstOrDefault(u => u.Id == id);
        }

        public ApplicationUser UpdateUser(ApplicationUser user)
        {
            _context.Update(user);
            _context.SaveChanges();

            return user;
        }
    }
}
