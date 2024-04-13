using TenantSubscriptionApp.Areas.Identity.Data;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TenantSubscriptionApp.Core;

namespace TenantSubscriptionApp.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly AuthDBContext _dbContext;
        private readonly MasterDbContext _masterDbContext;

        private readonly IConfiguration _configuration;

        public SubscriptionRepository(AuthDBContext dbContext, IConfiguration configuration, MasterDbContext masterdDbContext)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _masterDbContext = masterdDbContext;
        }

        public ICollection<TenantSubscription> GetSubscriptions(int organisationId)
        {
            List<TenantSubscription> subscriptions = new List<TenantSubscription>();

            subscriptions = _dbContext.TenantSubscriptions.Include(ts => ts.Organisation).Include(ts => ts.Application)
                                    .Where(ts => ts.OrganisationId == organisationId)
                                    .ToList();

            return subscriptions;
        }

        public async Task<bool> AddSubscription(int applicationId, ApplicationUser user)
        {
            bool isSubscribed = false;
            try
            {
                var existingSubscription = await _dbContext.TenantSubscriptions
                                                .Where(t => t.ApplicationId == applicationId && t.OrganisationId == user.OrganisationId)
                                                .FirstOrDefaultAsync();
                if (existingSubscription == null)
                {
                    var templateConnectionString = _configuration.GetConnectionString("MasterConnectionString");

                    var organisationWithoutSpaces = user.Organisation.Name.Replace(" ", "");
                    var applicationDbName = $"{Enum.GetName(typeof(DBNames), applicationId)}";

                    var dbName = organisationWithoutSpaces + applicationDbName;


                    // Create a copy of the template connection string
                    var connectionStringBuilder = new SqlConnectionStringBuilder(templateConnectionString)
                    {
                        InitialCatalog = dbName
                    };

                    // Create subscription entity
                    var subscription = new TenantSubscription
                    {
                        UserId = user.Id,
                        OrganisationId = user.Organisation.Id,
                        ApplicationId = applicationId,
                        ConnectionString = connectionStringBuilder.ToString(),
                    };

                    // Create the new database on Azure SQL
                    var createDatabaseQuery = $"CREATE DATABASE {dbName} AS COPY OF {applicationDbName}";

                    var command = _masterDbContext.Database.GetDbConnection().CreateCommand();
                    command.CommandTimeout = 500;

                    _masterDbContext.Database.ExecuteSqlRawAsync(
                        createDatabaseQuery
                    );

                    // Add the subscription to the master database
                    _dbContext.TenantSubscriptions.Add(subscription);
                    await _dbContext.SaveChangesAsync();

                    isSubscribed = true;
                }
                return isSubscribed;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckDbCreated(TenantSubscription input)
        {
            bool isCreated = false;
            int dbCount; 
            var templateConnectionString = _configuration.GetConnectionString("MasterConnectionString");
            try
            {
                string? organisationName = input.Organisation.Name.Replace(" ", "");

                string dbName = $"{organisationName}{Enum.GetName(typeof(DBNames), input.ApplicationId)}";

                string checkQuery = $"SELECT Count(*) AS Count FROM sys.databases WHERE name = @dbName";

                using SqlConnection conn = new(templateConnectionString);

                //using var cmd = conn.CreateCommand(checkQuery);

                using (var cmd  = conn.CreateCommand())
                {
                    cmd.CommandText = checkQuery;

                    cmd.Parameters.AddWithValue("@dbName", dbName);

                    await conn.OpenAsync();

                    var count = await cmd.ExecuteScalarAsync();
                    dbCount = Convert.ToInt16(count);

                    await conn.CloseAsync();
                }

                if (dbCount > 0 && (DateTime.UtcNow - input.CreatedAt) > TimeSpan.FromMinutes(2))
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
    public class DatabaseExistsResult
    {
        public int Count { get; set; }
    }
}
