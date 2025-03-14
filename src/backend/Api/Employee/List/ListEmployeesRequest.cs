using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Employee.List;

public record ListEmployeesRequest : IRequest<Result<ListEmployeesResponse>>;