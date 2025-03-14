using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Project.List;

public record ListProjectsRequest : IRequest<Result<ListProjectsResponse>>;
