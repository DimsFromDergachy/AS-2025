using Ardalis.Result;
using MediatR;

namespace AS_2025.Api.TableControlsPresentation.List;

public record ListTableControlsPresentationsRequest : IRequest<Result<ListTableControlsPresentationsResponse>>;
