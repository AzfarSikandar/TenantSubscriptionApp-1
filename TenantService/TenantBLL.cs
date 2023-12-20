using TenantSubscriptionApp.Data;
using Microsoft.Data.SqlClient;

namespace TenantSubscriptionApp.TenantService
{
    public class TenantBLL : ITenantBLL
    {
        private readonly IConfiguration _configuration;

        private readonly AuthDBContext _context;

        public TenantBLL(IConfiguration configuration, AuthDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<string> CreateConnectionString(string appName)
        {
            var templateConnectionString = _configuration.GetConnectionString("TemplateConnectionString");
            SqlConnectionStringBuilder connectionString = new(templateConnectionString);
            connectionString.InitialCatalog = appName;
            return connectionString.ToString();
        }

        public async Task<string> GetTenantConnectionStringByUserId(string userId, string appName)
        {
            string response = " ";

            //try
            //{
            //    response = _context.TenantSubscriptions
            //        .Where(ts => ts.UserId == userId && ts.Application.Name == appName)
            //        .Select(ts => ts.ConnectionString)
            //        .FirstOrDefault();
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            return response;
        }
    }
}
