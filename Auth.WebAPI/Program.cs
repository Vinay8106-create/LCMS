using Auth.Application;
using Auth.Infra;
using Galaxy.ApiUtility;
using Galaxy.Application;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using Galaxy.Infra.Middleware;
using Galaxy.MultiTenant;
using Galaxy.MultiTenant.DbPlugin;
using Galaxy.Utility;
using ITGAcc.Integration.Application;
using ITGAccounts.S2SLogic;
using LCMS.S2SLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------
// Basic Configuration & Services
// -----------------------------
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorizationPolicy();
builder.Services.AddJwtAuthentication();

// Middleware Dependencies
builder.Services.AddScoped<BaseRequestProfile>();
builder.Services.AddScoped<JsonToken>();
builder.Services.AddScoped<BaseRequestProfileMiddleware>();
builder.Services.AddScoped<JsonTokenMiddleware>();

// -----------------------------
// Tenant DB Provider
// -----------------------------
builder.Services.AddScoped<ITenantProvider, MultiTenantDbPlugIn>(provider =>
    new MultiTenantDbPlugIn(
        provider.GetRequiredService<BaseRequestProfile>(),
        builder.Configuration.GetConfigurationFromDataBaseSettings("DBProvider"),
        builder.Configuration.GetDecryptedConnectionString(),
        provider.GetRequiredService<IMemoryCache>())
);

// -----------------------------
// Table Caching
// -----------------------------
var cacheExpiration = builder.Configuration.GetValue<TimeSpan>("TableCaching:CacheExpirationTime");
builder.Services.RegisterTableCaching(
    builder.Configuration.GetConfigurationFromTableCaching("Tables"),
    cacheExpiration
);

// -----------------------------
// DbContext & Scoped Services
// -----------------------------
builder.Services.AddDbContext<AuthDBContext>();
builder.Services.AddScoped<IQueryable<User>>(x => x.GetRequiredService<AuthDBContext>().Set<User>().Cast<User>());

// Custom Dependency Registrations
builder.Services.RegisterInfra();
builder.Services.RegisterBaseApplicationServices();
builder.Services.RegisterS2SLogic();
builder.Services.RegisterAuthServices();
builder.Services.RegisterAccountsS2SLogic();
builder.Services.RegisterBaseAccountsServices();

// Unified DbContext Interface
builder.Services.AddScoped<ITGDbContext>(x => x.GetRequiredService<AuthDBContext>());

// -----------------------------
// JSON Serialization Settings
// -----------------------------
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// -----------------------------
// Logging
// -----------------------------
builder.Logging.AddConsole(options => {
    options.LogToStandardErrorThreshold = LogLevel.Debug;
});

// -----------------------------
// API Behavior Configuration
// -----------------------------
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

// -----------------------------
// Swagger Setup
// -----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddSwaggerGen(options => {
    options.UseAllOfToExtendReferenceSchemas();
    options.EnableAnnotations();
    options.SupportNonNullableReferenceTypes();
});


// -----------------------------
// Hosting Setup
// -----------------------------
var hostingMechanism = builder.Configuration.GetConfigurationFromAppSettings("HostingMechanism");
if (hostingMechanism == "Kestrel")
{
    builder.WebHost.UseKestrel(options => {
        options.Limits.MaxRequestBodySize = long.MaxValue;
        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);

        var portString = builder.Configuration.GetConfigurationFromAppSettings("ListeningPort");

        var httpPort = int.TryParse(portString, out var parsedPort) ? parsedPort : 1807;
        options.ListenAnyIP(httpPort);

        options.ListenAnyIP(7279, listenOptions => {
            listenOptions.UseHttps();
        });
    });
}
else
{
    builder.WebHost.UseIISIntegration();
}

// -----------------------------
// Global Exception Handling
// -----------------------------
builder.Services.AddGlobalExceptionHandler();

var app = builder.Build();

// -----------------------------
// Middleware Pipeline
// -----------------------------
app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

var environment = builder.Configuration.GetConfigurationFromAppSettings("Environment");
if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint(
            builder.Configuration.GetConfigurationFromAppSettings("SwaggerPath"),
            "API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<JsonTokenMiddleware>();
app.UseMiddleware<BaseRequestProfileMiddleware>();
app.UseExceptionHandler();

app.MapControllers();
app.Run();
