using TenantSubscriptionApp.Core;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace TenantSubscriptionApp.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAllUsers}")]
        public async Task<IActionResult> SubscriptionIndex()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _unitOfWork.User.GetUser(userId);

            var subscriptions = _unitOfWork.Subscription.GetSubscriptions(user.OrganisationId);

            foreach (var subscription in subscriptions)
            {

                bool isCreated = await _unitOfWork.Subscription.CheckDbCreated(subscription);
                subscription.IsActive = isCreated;

            }

            return View(subscriptions);
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAdminOrManager}")]
        public IActionResult AddSubscription()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _unitOfWork.User.GetUser(userId);

            var subscribedApps = _unitOfWork.Subscription.GetSubscriptions(user.OrganisationId);


            ViewBag.SubscribedApps = subscribedApps.Count > 0 ? subscribedApps.AsEnumerable().Select(ts => ts.Application).ToList() : new List<Application>();

            var availableApplications = _unitOfWork.Application.GetApplications().ToList();

            return View(availableApplications);
        }

        [Authorize(Policy = $"{Constants.Policies.RequireAdminOrManager}")]
        [HttpPost]
        public async Task<IActionResult> Add(int applicationId)
        {

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var user = _unitOfWork.User.GetUser(userId);

            var isCreated = await _unitOfWork.Subscription.AddSubscription(applicationId, user);

            return RedirectToAction("SubscriptionIndex");
        }
    }
}
