using AS_2025;

// ReSharper disable once UseObjectOrCollectionInitializer
var templates = new Dictionary<string, string>();

#region DOMAIN

// ReSharper disable once UseRawString
templates["generated/Domain/Entities/{{ ModelName }}.cs"] = @"using AS_2025.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace AS_2025.Domain.Entities;

public record {{ ModelName }} : Entity<Guid>, IIdentifiableEntity<string>
{
    public sealed override Guid Id { get; init; } = Guid.CreateVersion7();

    public string ExternalId { get; init; } = string.Empty;
}
";

#endregion

#region EF CONFIGURATION

// ReSharper disable once UseRawString
templates["generated/Database/Configuration/{{ ModelName }}Configuration.cs"] = @"using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AS_2025.Database.Configuration;

public class {{ ModelName }}Configuration : IEntityTypeConfiguration<{{ ModelName }}>
{
    public void Configure(EntityTypeBuilder<{{ ModelName }}> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).HasConversion<GuidToStringConverter>();

        builder.HasIndex(x => x.ExternalId).IsUnique();
    }
}
";

#endregion

#region APPLICATION SERVICE

// ReSharper disable once UseRawString
templates["generated/ApplicationServices/{{ ModelName }}Service.cs"] = @"using AS_2025.Common;
using AS_2025.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.ApplicationServices;

public class {{ ModelName }}Service
{
    private readonly IContext _context;

    public {{ ModelName }}Service(IContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<{{ ModelName }}>> ListAsync(CancellationToken cancellationToken)
    {
        return await _context.{{ ModelName }}s
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
";

#endregion

#region LIST REPR

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/List/List{{ ModelName }}sHandler.cs"] = @"using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.{{ ModelName }}.List;

public class List{{ ModelName }}sHandler : IRequestHandler<List{{ ModelName }}sRequest, Result<List{{ ModelName }}sResponse>>
{
    private readonly {{ ModelName }}Service _service;

    public List{{ ModelName }}sHandler({{ ModelName }}Service service)
    {
        _service = service;
    }

    public async Task<Result<List{{ ModelName }}sResponse>> Handle(List{{ ModelName }}sRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken))
            .Select(Mapper.ToListItem)
            .ToList();
        return new List{{ ModelName }}sResponse
        {
            Items = items
        };
    }
}
";

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/List/List{{ ModelName }}sItem.cs"] = @"using AS_2025.Schema.List;

namespace AS_2025.Api.{{ ModelName }}.List;

public record List{{ ModelName }}sItem
{
    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public Guid Id { get; init; }

    [ListColumnSchema(VisibilityType = ListColumnVisibilityType.Hidden, DisplayType = ListColumnDisplayType.None)]
    public string Url { get; init; }
}
";

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/List/List{{ ModelName }}sRequest.cs"] = @"using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.{{ ModelName }}.List;

public record List{{ ModelName }}sRequest : IRequest<Result<List{{ ModelName }}sResponse>>;
";

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/List/List{{ ModelName }}sResponse.cs"] = @"
namespace AS_2025.Api.{{ ModelName }}.List;

public record List{{ ModelName }}sResponse
{
    public IReadOnlyCollection<List{{ ModelName }}sItem> Items { get; init; } = Array.Empty<List{{ ModelName }}sItem>();
}
";

#endregion

#region REFERENCE LIST REPR

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/ReferenceList/ReferenceList{{ ModelName }}sHandler.cs"] = @"using Ardalis.Result;
using AS_2025.ApplicationServices;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.{{ ModelName }}.ReferenceList;

public class ReferenceList{{ ModelName }}sHandler : IRequestHandler<ReferenceList{{ ModelName }}sRequest, Result<ReferenceListResponse>>
{
    private readonly {{ ModelName }}Service _service;
    private readonly ReferenceListBuilder _referenceListBuilder;

    public ReferenceList{{ ModelName }}sHandler({{ ModelName }}Service service, ReferenceListBuilder referenceListBuilder)
    {
        _service = service;
        _referenceListBuilder = referenceListBuilder;
    }

    public async Task<Result<ReferenceListResponse>> Handle(ReferenceList{{ ModelName }}sRequest request, CancellationToken cancellationToken)
    {
        var items = (await _service.ListAsync(cancellationToken)).ToList();
        return new ReferenceListResponse
        {
            Items = _referenceListBuilder.Build(items)
        };
    }
}
";

// ReSharper disable once UseRawString
templates["generated/Api/{ModelName}/ReferenceList/ReferenceList{{ ModelName }}sRequest.cs"] = @"using Ardalis.Result;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.{{ ModelName }}.ReferenceList;

public record ReferenceList{{ ModelName }}sRequest : IRequest<Result<ReferenceListResponse>>;
";

#endregion

#region REGISTER ENDPOINTS

// ReSharper disable once UseRawString
templates["generated/Api/{{ ModelName }}/{{ ModelName }}Endpoints.cs"] = @"using Ardalis.Result.AspNetCore;
using AS_2025.Api.{{ ModelName }}.List;
using AS_2025.Api.{{ ModelName }}.ReferenceList;
using AS_2025.Schema.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.{{ ModelName }};

public static class {{ ModelName }}Endpoints
{
    public static void Map{{ ModelName }}Endpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup(""api/{{ modelName }}"")
            .WithTags(""{{ ModelName }}"");

        group.MapGet(""/list"", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] List{{ ModelName }}sRequest? request) =>
        {
            var result = await mediator.Send(request ?? new List{{ ModelName }}sRequest());
            return result.ToMinimalApiResult();
        });

        group.MapGet(""/list/schema"", ([FromServices] ListSchemaModelBuilder listSchemaModelBuilder) => listSchemaModelBuilder.Build<List{{ ModelName }}sItem>());

        group.MapGet(""/reference-list"", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ReferenceList{{ ModelName }}sRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ReferenceList{{ ModelName }}sRequest());
            return result.ToMinimalApiResult();
        });
    }
}
";

#endregion

#region REPR MAPPER

// ReSharper disable once UseRawString
templates["generated/Api/{{ ModelName }}/Mapper.cs"] = @"using AS_2025.Api.{{ ModelName }}.List;
using Riok.Mapperly.Abstractions;

namespace AS_2025.Api.{{ ModelName }};

[Mapper(AllowNullPropertyAssignment = true, ThrowOnMappingNullMismatch = false)]
public static partial class Mapper
{
    [MapProperty(nameof(Domain.Entities.{{ ModelName }}.Id), nameof(List{{ ModelName }}sItem.Url), Use = nameof(GuidToUrl))]
    public static partial List{{ ModelName }}sItem ToListItem(Domain.Entities.{{ ModelName }} entity);

    private static string GuidToUrl(Guid id)
    {
        return $""/{{ modelName }}/{id}"";
    }
}
";

#endregion

try
{
    new Generator().Generate(templates, new List<string> { "Client", "Project", "Task", "Team" });
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}