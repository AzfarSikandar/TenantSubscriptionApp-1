using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace TenantSubscriptionApp.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AuthDBContext _dbContext;

        private readonly IConfiguration _configuration;

        public SubscriptionRepository(AuthDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public  ICollection<TenantSubscription> GetSubscriptions(int organisationId)
        {
            List<TenantSubscription> subscriptions = new List<TenantSubscription>();

            subscriptions = _dbContext.TenantSubscriptions.Include(ts => ts.Organisation).Include(ts => ts.Application)
                                    .Where(ts => ts.OrganisationId == organisationId)
                                    .ToList();

            return subscriptions;
        }


        public async Task<bool> AddSubscription(int applicationId, ApplicationUser user)
        {
            bool iscreated = false;
            try
            {
                //var applicationId = _context.Applications.Where(ap => ap.Name == appName).Select(ap => ap.Id).FirstOrDefault();
                var organization = user.Organisation;

                var templateConnectionString = _configuration.GetConnectionString("TemplateConnectionString");

                var connectionStringBuilder = new SqlConnectionStringBuilder(templateConnectionString);

                var organisationRemoveWhiteSpaces = organization.Name.Replace(" ", "");

                using (SqlConnection conn = new(templateConnectionString))
                {
                    string dbName = "";
                    switch (applicationId)
                    {
                        case 1:
                            dbName = "ERPSGS";
                            break;
                        case 2:
                            dbName = "PayrollDB_Azure";
                            break;
                    }
                    connectionStringBuilder.InitialCatalog = organisationRemoveWhiteSpaces + dbName;

                    var subscription = new TenantSubscription
                    {
                        UserId = user.Id,
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

                    await _dbContext.TenantSubscriptions.AddAsync(subscription);
                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

                throw;
            }
            return iscreated;
        }
       
        public async Task<bool> CheckDbCreated(TenantSubscription input)
        {
            bool isCreated = false;
            int count = 0;
            var templateConnectionString = _configuration.GetConnectionString("TemplateConnectionString");
            try
            {
                using (SqlConnection conn = new(templateConnectionString))
                {
                    string? organisationName = input.Organisation.Name.Replace(" ", "");

                    string appName = "";

                    switch (input.Application.Id)
                    {
                        case 1:
                            appName = "ERPSGS";
                            break;
                        case 2:
                            appName = "PayrollDB_Azure";
                            break;
                    }

                    string dbName = organisationName + appName;
                    string checkQuery = $"SELECT Count(*) AS Count FROM sys.databases WHERE name = @dbName";

                    using SqlCommand cmd = new(checkQuery, conn);

                    cmd.Parameters.AddWithValue("@dbName", dbName);

                    await conn.OpenAsync();

                    var rowCount = await cmd.ExecuteScalarAsync();

                    count = Convert.ToInt16(rowCount);
                    await conn.CloseAsync();
                }

                if (count > 0 && (DateTime.UtcNow - input.CreatedAt) > TimeSpan.FromMinutes(2))
                {
                    isCreated = true;
                    var subscriptionToUpdate = _dbContext.TenantSubscriptions.Find(input.Id);

                    if (subscriptionToUpdate != null)
                    {
                        subscriptionToUpdate.IsActive = true;
                        _dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return isCreated;
        }
    }
}
