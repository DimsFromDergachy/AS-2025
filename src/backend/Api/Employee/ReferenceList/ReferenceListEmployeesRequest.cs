using Ardalis.Result;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Employee.ReferenceList;

public record ReferenceListEmployeesRequest : IRequest<Result<ReferenceListResponse>>;