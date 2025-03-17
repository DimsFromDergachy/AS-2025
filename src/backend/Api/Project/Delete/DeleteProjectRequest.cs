using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Project.Delete;

public record DeleteProjectRequest : IRequest<Result<DeleteProjectResponse>>
{
    public Guid Id { get; init; }
}