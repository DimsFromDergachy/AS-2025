using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Task.Delete;

public record DeleteTaskRequest : IRequest<Result<DeleteTaskResponse>>
{
    public Guid Id { get; init; }
}