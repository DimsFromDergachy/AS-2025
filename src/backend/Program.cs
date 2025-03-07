using AS_2025.Api.Trait;
using AS_2025.Common;
using AS_2025.Database;
using AS_2025.Identity;
using AS_2025.Options;
using AS_2025.Repository;
using AS_2025.Schema;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddDatabaseHostedServices();
builder.Services.AddRepositories();
builder.Services.AddSchemaBuilders();

builder.Services.AddExceptionHandler<ExceptionHandler>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.Configure<Microsoft.AspNetCore.Mvc.JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseResponseCompression();

app.UseAuthentication();
app.UseAuthorization();

app.MapTraitEndpoints();

app.MapGroup("api/identity")
    .WithTags("Identity")
    .MapIdentityApi<ApplicationUser>();

app.UseHttpsRedirection();

app.Run();
