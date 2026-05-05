using Common.HttpClient;
using CRM.Application;
using CRM.Infra;
using Galaxy.ApiUtility;
using Galaxy.Application.Mapper;
using Galaxy.Domain.Models;
using Galaxy.Infra;
using Galaxy.Infra.Middleware;
using Galaxy.MultiTenant;
using Galaxy.MultiTenant.DbPlugin;
using Galaxy.Utility;
using LCMS.Persistence;
using LCMS.S2SLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.IgnoreReadOnlyProperties = true;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddAuthorizationPolicy();
builder.Services.AddJwtAuthentication();
builder.Services.AddScoped<BaseRequestProfile>();
builder.Services.AddScoped<JsonToken>();
builder.Services.AddScoped<AppMessageResolver>();
builder.Services.AddScoped<BaseRequestProfileMiddleware>();
builder.Services.AddScoped<JsonTokenMiddleware>();
builder.Services.AddHttpClient<IHttpServiceClient, HttpServiceClient>();
builder.Services.AddScoped<IS2SLogic, S2SLogic>();

builder.Services.AddDbContext<LCMSDbContext>();  ////
builder.Services.RegisterInfra();
builder.Services.RegisterMasterServices();

//Multi - Tenant Configuration
builder.Services.AddScoped<ITenantProvider, MultiTenantDbPlugIn>(provider =>
  new MultiTenantDbPlugIn(
  provider.GetRequiredService<BaseRequestProfile>(),
  builder.Configuration.GetConfigurationFromDataBaseSettings("DBProvider"),
  builder.Configuration.GetDecryptedConnectionString(),
  provider.GetRequiredService<IMemoryCache>())
);

//Caching
builder.Services.RegisterTableCaching(
  builder.Configuration.GetConfigurationFromTableCaching("Tables"),
  builder.Configuration.GetValue<TimeSpan>("TableCaching:CacheExpirationTime")
);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddSwaggerGen(options => {
    options.UseAllOfToExtendReferenceSchemas();
    options.EnableAnnotations();
    options.SupportNonNullableReferenceTypes();
});

// ------------------ API Behavior ------------------
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.SuppressModelStateInvalidFilter = true;
});

// ------------------ Logging ------------------
builder.Logging.AddConsole(options => {
    options.LogToStandardErrorThreshold = LogLevel.Debug;
});

// Configure hosting mechanism
if (builder.Configuration.GetConfigurationFromAppSettings("HostingMechanism") == "Kestrel")
{
    builder.WebHost.UseKestrel(options => {
        options.Limits.MaxRequestBodySize = long.MaxValue;
        options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
        options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);


        var portString = builder.Configuration.GetConfigurationFromAppSettings("ListeningPort");
        options.ListenAnyIP(int.TryParse(portString, out var parsedPort) ? parsedPort : 1804);
    });
}
else
{
    builder.WebHost.UseIISIntegration();
}
builder.Services.AddGlobalExceptionHandler();

var app = builder.Build();


// Enable CORS for UI integration
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Enable Swagger UI in Development
var environment = builder.Configuration.GetConfigurationFromAppSettings("Environment");
if (string.Equals(environment, "Development"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint(builder.Configuration.GetConfigurationFromAppSettings("SwaggerPath"), "API V1");
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
