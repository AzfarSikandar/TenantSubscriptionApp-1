using TenantSubscriptionApp.Core.ViewModels;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Helper;
using TenantSubscriptionApp.TenantService;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Claims;

namespace TenantSubscriptionApp.Country
{
    public class CountryService : ICountryService
    {
        private readonly ITenantBLL _tenantService;
        private readonly AuthDBContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        //private readonly _userManager

        private readonly string _connectionString;

        public CountryService(ITenantBLL tenantService, AuthDBContext context, IHttpContextAccessor httpContextAccessor)   
        {
            _tenantService = tenantService;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _connectionString = GetConnectionString();

        }

        private string GetConnectionString()
        {
            //string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //var connectionString = _context.TenantSubscriptions
            //        .Where(ts => ts.UserId == userId && ts.ApplicationName == "ERP System")
            //        .Select(ts => ts.ConnectionString)
            //        .FirstOrDefault();

            //return connectionString;
            return " ";
        }
        

        public async Task<List<CountryViewModel>> GetAllCountries(string userId)
        {
            var countries = new List<CountryViewModel>();

            var connectionString = GetConnectionString();

            //var connectionString = _context.TenantSubscriptions
            //        .Where(ts => ts.UserId == userId && ts.ApplicationName == "ERP System")
            //        .Select(ts => ts.connectionString)
            //        .FirstOrDefault();
            SqlDataReader reader = null;

            //DataSet countriesDataset = new DataSet();

            DataTable countriesTable = new();

            using (var conn = new SqlConnection(_connectionString))
            {
                var getAllCountriesQuery = $"SELECT Id, EnglishName, ArabicName, Code, ZipCode, Iso3 FROM [lkp].[County] WHERE IsDeleted = 0";

                using var command = new SqlCommand(getAllCountriesQuery, conn);

                command.CommandType = CommandType.Text;


                await conn.OpenAsync();
                reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                countriesTable.Load(reader, LoadOption.PreserveChanges);
            }

            countries = countriesTable.AsEnumerable()
                                        .Select(row => row.ToObject<CountryViewModel>())
                                        .ToList();

            return countries;
        }

        public async Task<CountryViewModel> GetCountry(int countryId)
        {
            var response = new CountryViewModel();
            SqlDataReader reader = null;
            DataTable countryTable = new();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    var getCountry = $"SELECT Id, EnglishName, ArabicName, Code, ZipCode, Iso3 FROM [lkp].[County] WHERE Id = @id AND IsDeleted = 0";

                    using var command = new SqlCommand(getCountry, conn);

                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", countryId);


                    await conn.OpenAsync();
                    reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

                    countryTable.Load(reader, LoadOption.PreserveChanges);
                }

                response = countryTable.AsEnumerable()
                                            .Select(row => row.ToObject<CountryViewModel>())
                                            .FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }


        public async Task<int> AddCountry(CountryInputModel country)
        {

            var connectionString = GetConnectionString();
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            int response;


            using (SqlConnection conn = new(connectionString))
            {
                var insertQuery = $"DECLARE @countryId int = (SELECT Id FROM [lkp].[County] WHERE EnglishName = @englishName AND ArabicName = N'{country.ArabicName}' AND ZipCode = @zipCode AND Iso3 = @iso3)" +
                    $"\nIF (@countryId > 0)" +
                    $"\nBEGIN " +
                    $"\n\t UPDATE [lkp].[County] SET [IsDeleted] = 0 WHERE EnglishName = @englishName AND ArabicName = N'{country.ArabicName}' AND Code = @code  AND ZipCode = @zipCode AND Iso3 = @iso3\nEND" +
                    $"\nELSE" +
                    $"\nBEGIN" +
                    $"\n\tINSERT INTO [lkp].[County] (EnglishName, ArabicName, Code, ZipCode, Iso3, CreatedBy, CreationDate, IsDeleted) " +
                    $"\n\tValues(@englishName, @arabicname, @code, @zipCode, @iso3, 1, GETUTCDATE(), 0)" +
                    $"\nEND";
                using var cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@englishName", country.EnglishName);
                cmd.Parameters.AddWithValue("@arabicname", country.ArabicName);
                cmd.Parameters.AddWithValue("@code", country.Code);
                cmd.Parameters.AddWithValue("@zipCode", country.ZipCode);
                cmd.Parameters.AddWithValue("@iso3", country.Iso3);

                await conn.OpenAsync();

                response = await cmd.ExecuteNonQueryAsync();
            }

            return response;
        }

        public async Task<int> DeleteCountry(int id)
        {
            int response = 0;

            //string connectionString = GetConnectionString();

            try
            {
                string deleteQuery = $"Update [lkp].[County] \n\t SET [IsDeleted] = 1 WHERE Id = @id";

                using (SqlConnection conn = new(_connectionString))
                {
                    using SqlCommand command = new(deleteQuery, conn);

                    command.Parameters.AddWithValue("@id", id);

                    await conn.OpenAsync();

                    response = await command.ExecuteNonQueryAsync();

                    await conn.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }


        public async Task<int> EditCountry(CountryViewModel country)
        {
            int response = 0;

            string updateQuery = $"Update [lkp].[County] " +
                $"\n\tSET [EnglishName] = @englishName, [ArabicName] = @arabicName, [Code]= @code, [ZipCode] = @zipCode, [Iso3] = @iso3, [ModifiedBy] = 1, [ModifiedDate] = GETUTCDATE() WHERE Id = @id";

            using (SqlConnection conn = new(_connectionString))
            {
                using SqlCommand command = new(updateQuery, conn);

                command.Parameters.AddWithValue("@id", country.Id);
                command.Parameters.AddWithValue("@englishName", country.EnglishName);
                command.Parameters.AddWithValue("@arabicname", country.ArabicName);
                command.Parameters.AddWithValue("@code", country.Code);
                command.Parameters.AddWithValue("@zipCode", country.ZipCode);
                command.Parameters.AddWithValue("@iso3", country.Iso3);

                await conn.OpenAsync();

                response = await command.ExecuteNonQueryAsync();

                await conn.CloseAsync();

            }

            return response;
        }

    }
}
