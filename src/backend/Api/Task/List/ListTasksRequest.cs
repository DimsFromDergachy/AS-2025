using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Task.List;

public record ListTasksRequest : IRequest<Result<ListTasksResponse>>;
