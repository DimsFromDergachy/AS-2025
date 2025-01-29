using Ardalis.Result;
using AS_2025.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Api.Trait.List;

public class ListTraitsHandler : IRequestHandler<ListTraitsRequest, Result<ListTraitsResponse>>
{
    private readonly IContext _context;

    public ListTraitsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<ListTraitsResponse>> Handle(ListTraitsRequest request, CancellationToken cancellationToken)
    {
        var items = await _context.Traits
            .OrderBy(x => x.Code)
            .Select(x => Mapper.ToListTraitsItem(x))
            .ToListAsync(cancellationToken);
        return new ListTraitsResponse
        {
            Items = items
        };
    }
}