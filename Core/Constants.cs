namespace TenantSubscriptionApp.Core
{
    public static class Constants
    {

        public static class Roles
        {
            public const string Administrator = "Administrator";
            public const string Manager = "Manager";
            public const string User = "User";
        }

        public static class Policies
        {
            public const string RequireAdmin = "RequireAdmin";
            public const string RequireManager = "RequireManager";
            public const string RequireAllUsers = "AllUsers";
            public const string RequireAdminOrManager = "RequireAdminOrManager";
        }

        public static class Database
        {
            public const string ERPDB = "ERPSGS";
            public const string HRDB = "PayrollDB_Azure";
        }
    }

}
