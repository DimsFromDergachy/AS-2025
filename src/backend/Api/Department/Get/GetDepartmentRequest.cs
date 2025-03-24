using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Department.Get;

public record GetDepartmentRequest : IRequest<Result<GetDepartmentResponse>>
{
    public Guid Id { get; init; }
}