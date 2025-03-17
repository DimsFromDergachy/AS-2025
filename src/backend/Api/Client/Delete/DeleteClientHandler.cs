using Ardalis.Result;
using AS_2025.ApplicationServices;
using MediatR;

namespace AS_2025.Api.Client.Delete;

public class DeleteClientHandler : IRequestHandler<DeleteClientRequest, Result<DeleteClientResponse>>
{
    private readonly ClientService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteClientHandler(
        ClientService service, 
        IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<DeleteClientResponse>> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(request, cancellationToken);
        return new Result<DeleteClientResponse>(new DeleteClientResponse());
    }
}