using Enyim.Caching.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using QueueManagementSystem.MVC.Data;
using QueueManagementSystem.MVC.Services;
using QueueManagementSystem.MVC.Components;
using Serilog.Events;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console()
    .WriteTo.File(
        path: "logs/qms_.log",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; }); ;

builder.Services.AddFastReport();
builder.Services.AddBlazorBootstrap();

builder.Services.AddAuthentication().AddCookie("MyCookieScheme", options => {
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<ITicketService, TicketService>();
builder.Services.AddSingleton<IReportService, ReportService>();
builder.Services.AddScoped<IAuthService, AuthService>(); ;
builder.Services.AddScoped<ISmsService, SmsService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<IFingerprintService, FingerprintService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IDbContextManager, DbContextManager>();
builder.Services.AddHostedService<QueueCleanupBackgroundService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ServicePointService>();
builder.Services.AddScoped<PrivilegeService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<SystemUsersService>();
builder.Services.AddDbContextFactory<QueueManagementSystemContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<QueueManagementSystemContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//builder.Services.AddEnyimMemcached(options => options.AddServer("localhost", 11211));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

PrivilegeSeeder.SeedPrivileges(app);

// REVIEW: Seeding is done for development ease but shouldn't be here in production
//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<QueueManagementSystemContext>(); //TODO: use try clause incase service does not exist
//    QueueManagementSystemContextSeeder.SeedServiceCategories(context);
//    QueueManagementSystemContextSeeder.SeedServices(context);
//    QueueManagementSystemContextSeeder.SeedServicePoints(context);
//    QueueManagementSystemContextSeeder.SeedServiceProviders(context);
//    QueueManagementSystemContextSeeder.ResetQueue(context);
//}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();

app.UseFastReport();

try
{
    Log.Information("Starting web application");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
