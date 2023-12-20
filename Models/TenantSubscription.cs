using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TenantSubscriptionApp.Areas.Identity.Data;

namespace TenantSubscriptionApp.Models
{
    public class TenantSubscription
    {
        public int Id { get; set; }

        [ForeignKey("Organisation")]
        public int OrganisationId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey("Application")]
        public int ApplicationId { get; set; }

        public Application Application { get; set; }

        public string ConnectionString { get; set; }

        public Organisation Organisation { get; set; }

        [DefaultValue(false)]
        public bool IsActive { get; set; }

        [Column(TypeName = "datetime2")]
        [DefaultValue("GETUTCDATE()")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
