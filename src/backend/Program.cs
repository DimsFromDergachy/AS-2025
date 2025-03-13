using AS_2025.Common;
using AS_2025.Database;
using AS_2025.Identity;
using AS_2025.Options;
using AS_2025.Repository;
using AS_2025.Schema;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using AS_2025.Api.Department;
using AS_2025.Api.Employee;
using AS_2025.Api.Menu;
using AS_2025.Api.Team;
using AS_2025.Api.Utils;
using AS_2025.ApplicationServices;
using AS_2025.Exceptions;
using AS_2025.HostedServices;
using AS_2025.Import;
using AS_2025.ReferenceItem;
using AS_2025.Tags;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", true);

builder.Services.AddOpenApi();

builder.Services.AddAuthorization();

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
});

builder.Services.AddProblemDetails();

var app = builder.Build();

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

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.Run();
