using TenantSubscriptionApp.Core;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace TenantSubscriptionApp.Controllers
{
    public class DashboardController : Controller
    {
        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
