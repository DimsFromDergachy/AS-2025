using Ardalis.Result;
using AS_2025.Common;
using MediatR;

namespace AS_2025.Api.Trait.Create;

public class CreateTraitHandler : IRequestHandler<CreateTraitRequest, Result<TraitViewModel>>
{
    private readonly IContext _context;

    public CreateTraitHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<TraitViewModel>> Handle(CreateTraitRequest request, CancellationToken cancellationToken)
    {
        var created = Mapper.ToTraitEntity(request);

        _context.Traits.Add(created);
        await _context.SaveChangesAsync(cancellationToken);

        return Mapper.ToTraitViewModel(created);
    }
}