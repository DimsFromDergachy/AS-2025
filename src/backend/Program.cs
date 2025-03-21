using AS_2025.Common;
using AS_2025.Database;
using AS_2025.Identity;
using AS_2025.Options;
using AS_2025.Repository;
using AS_2025.Schema;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using AS_2025.Api.Algos;
using AS_2025.Api.Client;
using AS_2025.Api.Department;
using AS_2025.Api.Employee;
using AS_2025.Api.Menu;
using AS_2025.Api.Project;
using AS_2025.Api.TableControlsPresentation;
using AS_2025.Api.Task;
using AS_2025.Api.Team;
using AS_2025.Api.Utils;
using AS_2025.ApplicationServices;
using AS_2025.Channels;
using AS_2025.Exceptions;
using AS_2025.HostedServices;
using AS_2025.Hubs;
using AS_2025.Import;
using AS_2025.ReferenceItem;
using AS_2025.Tags;
using Microsoft.AspNetCore.Identity;
using AS_2025.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

builder.Services.AddOpenApi();

builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;

        options.SignIn.RequireConfirmedEmail = false;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<ApplicationUserRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthorizationPolicy.Anonymous, policy => policy.Requirements.Add(new AnonymousAuthorizationPolicy()));
    options.AddPolicy(AuthorizationPolicy.Authenticated, policy => policy.RequireRole(UserRole.Administrator));
    options.AddPolicy(AuthorizationPolicy.RoleIsAdministrator, policy => policy.RequireRole(UserRole.Administrator));
    options.AddPolicy(AuthorizationPolicy.RoleIsManager, policy => policy.RequireRole(UserRole.Manager));
    options.AddPolicy(AuthorizationPolicy.RoleIsUser, policy => policy.RequireRole(UserRole.User));
});

builder.Services.AddCompressionSetup();

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatRSetup();

var applicationOptions = new ApplicationOptions();
builder.Configuration.GetSection(ApplicationOptions.SectionKey).Bind(applicationOptions);

builder.Services.AddOptions(applicationOptions);
builder.Services.AddDatabase(applicationOptions);
builder.Services.AddApplicationServices();
builder.Services.AddHostedServices();
builder.Services.AddDataImportServices();
builder.Services.AddRepositories();
builder.Services.AddSchemaBuilders();
builder.Services.AddReferenceItems();
builder.Services.AddTags();

builder.Services.AddExceptionHandler<ApplicationExceptionHandler>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IncludeFields = true;
});

builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOnRemoteVM", policy =>
    {
        policy.WithOrigins($"https://{applicationOptions.HostName}:{applicationOptions.FrontendOriginPort}")
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromMinutes(30);
    options.KeepAliveInterval = TimeSpan.FromMinutes(15);
    options.EnableDetailedErrors = true;
});
builder.Services.AddChannels();
builder.Services.AddMiddlewares();

var app = builder.Build();

app.UseCors("AllowOnRemoteVM");

app.MapOpenApi();
app.MapScalarApiReference();

app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

//app.MapTraitEndpoints();
app.MapMenuEndpoints();
app.MapDepartmentEndpoints();
app.MapEmployeeEndpoints();
app.MapTeamEndpoints();
app.MapUtilsEndpoints();
app.MapClientEndpoints();
app.MapTaskEndpoints();
app.MapProjectEndpoints();
app.MapTableControlsPresentationEndpoints();
app.MapAlgosEndpoints();

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseMiddlewares();
app.MapHubs();

app.Run();
