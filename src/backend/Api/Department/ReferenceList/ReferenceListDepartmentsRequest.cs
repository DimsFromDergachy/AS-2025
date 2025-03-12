using Ardalis.Result;
using AS_2025.ReferenceItem;
using MediatR;

namespace AS_2025.Api.Department.ReferenceList;

public record ReferenceListDepartmentsRequest : IRequest<Result<ReferenceListResponse>>;