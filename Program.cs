using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TenantSubscriptionApp.Data;
using TenantSubscriptionApp.Core.Repositories;
using TenantSubscriptionApp.Repositories;
using TenantSubscriptionApp.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TenantSubscriptionConnectionString") ?? throw new InvalidOperationException("Connection string 'TenantSubscriptionConnectionString' not found.");
var masterConnectionString = builder.Configuration.GetConnectionString("MasterConnectionString") ?? throw new InvalidOperationException("Connection string 'MasterConnectionString' not found.");

builder.Services.AddDbContext<AuthDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<MasterDbContext>(options =>
{
    options.UseSqlServer(masterConnectionString);

});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDBContext>().AddDefaultTokenProviders();


//builder.Services.AddHttpContextAccessor();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;

});


//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<AuthDBContext>()
//    .AddDefaultTokenProviders();

// Add services to the container.
#region Authorization
AddAuthorizationPolicies(builder.Services);
#endregion
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//builder.Services.AddAuthorization(options => options.AddPolicy("UserOnly", policy => policy.RequireClaim("UserId")));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IOrganisationRepository, OrganisationRepository>();
builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//AddScoped();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
        name: "dashboard",
        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
        .RequireAuthorization();
//app.MapControllerRoute(
//        name: "organization",
//        pattern: "{controller=Dashboard}/{action=Index}/{id?}")
//        .RequireAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();


app.Run();

void AddAuthorizationPolicies(IServiceCollection services)
{
    services.AddAuthorization(options =>
    {
    options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager"));
    options.AddPolicy("RequireAdministrator", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("RequireAdminOrManager", policy => policy.RequireRole("Administrator", "Manager"));
    options.AddPolicy("AllUsers", policy => policy.RequireRole("User", "Administrator", "Manager"));
    });
}

//void AddScoped()
//{
    
//}
