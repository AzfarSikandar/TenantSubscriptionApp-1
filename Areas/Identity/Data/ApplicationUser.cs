using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace TenantSubscriptionApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
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

//public class UserRole : IdentityUserRole<string>
//{
//    public string UserId  { get; set; }

//    public string RoleId { get; set; }
//}

