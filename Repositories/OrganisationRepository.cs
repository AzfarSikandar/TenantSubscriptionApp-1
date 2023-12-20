using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TenantSubscriptionApp.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly AuthDBContext _dbContext;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrganisationRepository(AuthDBContext authDBContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = authDBContext;
            _userManager = userManager;
        }

        public ICollection<Organisation> GetOrganisations()
        {
            return _dbContext.Organisations.ToList();
        }

        public Organisation GetOrganisation(int id)
        {
            return _dbContext.Organisations.Where(og => og.Id == id).FirstOrDefault();
        }

        public async Task<bool> InsertOrganisation(Organisation input)
        {
            var isCreated = false;
            try
            {
                if (input != null)
                {
                    _dbContext.Organisations.Add(input);

                    await _dbContext.SaveChangesAsync();

                    isCreated = true;

                }
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    throw sqlException;
                }

                throw;
            }

            return isCreated;
        }

    }
}
