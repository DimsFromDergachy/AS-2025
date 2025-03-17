using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Client.Delete;

public record DeleteClientRequest : IRequest<Result<DeleteClientResponse>>
{
    public Guid Id { get; init; }
}