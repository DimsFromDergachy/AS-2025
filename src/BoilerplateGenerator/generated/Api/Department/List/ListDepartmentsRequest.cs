
using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Department.List;

public record ListDepartmentsRequest : IRequest<Result<ListDepartmentsResponse>>;
