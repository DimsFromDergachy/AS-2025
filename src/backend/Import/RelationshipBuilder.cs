using AS_2025.Common;

namespace AS_2025.Import;

public class RelationshipBuilder
{
    private readonly IContext _context;

    private readonly ImportDataContext _importDataContext;

    public RelationshipBuilder(IContext context, ImportDataContext importDataContext)
    {
        _context = context;
        _importDataContext = importDataContext;
    }

    public async Task BuildAsync(CancellationToken cancellationToken)
    {

    }
}