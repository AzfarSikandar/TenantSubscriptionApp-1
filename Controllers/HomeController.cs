using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TenantSubscriptionApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly AuthDBContext _authDBContext;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AuthDBContext authDBContext)
        {
            _logger = logger;
            this._userManager = userManager;
            _authDBContext = authDBContext;
        }

        public IActionResult Index()
        {
            var email = _userManager.GetUserName(this.User);
            
            int index = email.IndexOf("@");

            var userName = email.Substring(0, index);


            ViewData["UserName"]  = char.ToUpper(userName[0]) + userName.Substring(1);

            return View();
        }

        public IActionResult DashBoard()
        {


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}