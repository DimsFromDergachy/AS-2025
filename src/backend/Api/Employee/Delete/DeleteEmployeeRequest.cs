using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Employee.Delete;

public record DeleteEmployeeRequest : IRequest<Result<DeleteEmployeeResponse>>
{
    public Guid Id { get; init; }
}