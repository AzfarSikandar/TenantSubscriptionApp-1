using TenantSubscriptionApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TenantSubscriptionApp.Core.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }

        public IList<SelectListItem> Roles { get; set; }
    }
}
