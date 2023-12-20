namespace TenantSubscriptionApp.Models
{
    public class Application
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<TenantSubscription> Subscriptions { get; set; }
    }
}
