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
        private readonly IConfiguration _configuration;
        private readonly AuthDBContext _context;
        private readonly ISubscriptionRepository _subscriptionService;

        public DashboardController(AuthDBContext context, IConfiguration configuration, ISubscriptionRepository subscriptionService)
        {
            _context = context;
            _configuration = configuration;
            _subscriptionService = subscriptionService;
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> Subscription()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //user = _unitOfWork

            List<TenantSubscription> subscriptions = new List<TenantSubscription>();
            subscriptions = _context.TenantSubscriptions
                                    .Where(ts => ts.UserId == userId)
                                    .Select(row => new TenantSubscription
                                    {
                                        Id = row.Id,
                                        UserId = row.UserId,
                                        ApplicationId = row.ApplicationId,
                                        IsActive = row.IsActive,
                                        ConnectionString = row.ConnectionString,
                                        CreatedAt = row.CreatedAt,
                                        // Include the Application navigation property
                                        Application = new Application
                                        {
                                            // Include only the properties you need from Application
                                            Id = row.Application.Id,
                                            Name = row.Application.Name
                                        }
                                    })
                                    .ToList();


            for (int i = 0; i < subscriptions.Count(); i++)
            {
                if (!subscriptions[i].IsActive)
                {
                    bool isCreated = await _subscriptionService.CheckDbCreated(subscriptions[i]);
                    subscriptions[i].IsActive = isCreated;
                }
            }

            return View(subscriptions);
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> AddSubscription()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var subscribedApps = _context.TenantSubscriptions
                .Where(ts => ts.UserId == (userId))
                .Select(ts => ts.Application.Name)
                .ToList();

            ViewBag.SubscribedApps = subscribedApps;

            var availableApplications = _context.Applications.ToList();

            return View(availableApplications);
        }


        [Authorize(Policy = $"{Constants.Policies.RequireAdminOrManager}")]
        [HttpPost]
        public async Task<IActionResult> AddSubscription(string appName)
        {
            try
            {
                var applicationId = _context.Applications.Where(ap => ap.Name == appName).Select(ap => ap.Id).FirstOrDefault();

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var organization = _context.Users.Where(us => us.Id == userId).Select(us => us.Organisation).FirstOrDefault();

                var templateConnectionString = _configuration.GetConnectionString("TemplateConnectionString");

                var connectionStringBuilder = new SqlConnectionStringBuilder(templateConnectionString);

                var organisationRemoveWhiteSpaces = organization.Name.Replace(" ", "");

                using (SqlConnection conn = new(templateConnectionString))
                {
                    string dbName = "";
                    switch (appName)
                    {
                        case "ERP System":
                            dbName = "ERPSGS";
                            break;
                        case "HR Management System":
                            dbName = "PayrollDB_Azure";
                            break;
                    }
                    connectionStringBuilder.InitialCatalog = organisationRemoveWhiteSpaces + dbName;

                    var subscription = new TenantSubscription
                    {
                        UserId = userId,
                        OrganisationId = organization.Id,
                        ApplicationId = applicationId,
                        ConnectionString = connectionStringBuilder.ToString(),

                    };
                    //var dbCount = _context.TenantSubscriptions
                    //                .Where(sub => sub.UserId == userId && sub.ApplicationName == appName)
                    //                .Count();
                    string backupQuery = $"CREATE DATABASE {organisationRemoveWhiteSpaces + dbName} AS COPY OF {dbName}";

                    //string dbExistQuery = $"SELECT Count(*) AS Count FROM sys.Databases WHERE name = @name";

                    using (SqlCommand command = new SqlCommand(backupQuery, conn))
                    {
                        await conn.OpenAsync();

                        command.ExecuteNonQueryAsync();
                        //command.CommandTimeout = 300;
                        //await conn.CloseAsync();
                    }

                    await _context.TenantSubscriptions.AddAsync(subscription);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception ex)
            {
                //ViewBag.ErrorMessage = ex.Message;  
                //return View("Error");
                throw ex;
            }

            return RedirectToAction("Subscription");
        }
    }
}
