using System.ComponentModel.DataAnnotations.Schema;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Identity;

namespace TenantSubscriptionApp.Areas.Identity.Data;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    [ForeignKey("Organisation")]
    public int OrganisationId { get; set; }

    public Organisation Organisation { get; set; }

    public List<TenantSubscription> Subscriptions { get; set; }
}
