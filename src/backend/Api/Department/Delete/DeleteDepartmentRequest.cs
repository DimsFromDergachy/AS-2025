using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Department.Delete;

public record DeleteDepartmentRequest : IRequest<Result<DeleteDepartmentResponse>>
{
    public Guid Id { get; init; }
}