using TenantSubscriptionApp.Areas.Identity.Data;
using MessagePack;
using Microsoft.EntityFrameworkCore;

namespace TenantSubscriptionApp.Models
{
    public class Organisation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<ApplicationUser> Users { get; set; }

        public List<TenantSubscription> Subscriptions { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CreatedBy { get; set; }
    }
}
