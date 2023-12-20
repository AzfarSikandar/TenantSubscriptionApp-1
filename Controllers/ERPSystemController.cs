using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.TenantService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TenantSubscriptionApp.Controllers
{
    public class ERPSystemController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly AuthDBContext _context;
        private readonly ITenantBLL _tenantService;

        public ERPSystemController(AuthDBContext context, IConfiguration configuration, ITenantBLL tenantService)
        {
            _context = context;
            _configuration = configuration;
            _tenantService = tenantService;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Countries() 
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult>

    }
}
