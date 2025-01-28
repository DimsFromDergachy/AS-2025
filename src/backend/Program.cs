using AS_2025.Api.Trait;
using AS_2025.Common;
using AS_2025.Database;
using AS_2025.Identity;
using AS_2025.Options;
using AS_2025.Repository;
using Scalar.AspNetCore;

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

builder.Services.AddExceptionHandler<ExceptionHandler>();

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

app.Run();
