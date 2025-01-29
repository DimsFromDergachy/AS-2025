using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.Trait.List;

public record ListTraitsRequest : IRequest<Result<ListTraitsResponse>>
{

}