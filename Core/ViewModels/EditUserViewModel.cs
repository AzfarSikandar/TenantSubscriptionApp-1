using TenantSubscriptionApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

namespace TenantSubscriptionApp.Core.ViewModels
{
    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }

        public IList<SelectListItem> Roles { get; set; }

        public SelectList OrganisationList { get; set; }

        [BindProperty]
        public int SelectedOrganisationId { get; set; }
    }
}
