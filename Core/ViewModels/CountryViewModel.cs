namespace TenantSubscriptionApp.Core.ViewModels
{
    public class CountryViewModel
    {
        public int Id { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public string Code { get; set; }
        public string ZipCode { get; set; }
        public string Iso3 { get; set; }
    }

    public class CountryInputModel
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public int Code { get; set; }
        public int ZipCode { get; set; }
        public string Iso3 { get; set; }

    }
}
