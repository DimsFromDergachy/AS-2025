using Ardalis.Result;
using AS_2025.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AS_2025.Api.Trait.Update;

public class UpdateTraitHandler : IRequestHandler<UpdateTraitRequest, Result<TraitViewModel>>
{
    private readonly IContext _context;

    public UpdateTraitHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<TraitViewModel>> Handle(UpdateTraitRequest request, CancellationToken cancellationToken)
    {
        var existing = await _context.Traits
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (existing is null)
        {
            return Result.NotFound();
        }

        existing.UpdateEntity(request);

        _context.Traits.Update(existing);
        await _context.SaveChangesAsync(cancellationToken);

        return Mapper.ToTraitViewModel(existing);
    }
}