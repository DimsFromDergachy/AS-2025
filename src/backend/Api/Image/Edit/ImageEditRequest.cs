using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AS_2025.Api.Image.Edit;

public record ImageEditRequest : IRequest<Result<ImageEditResponse>>
{
    public int X { get; init; }

    public int Y { get; init; }

    public string Text { get; init; }

    [FromForm]
    public IFormFile File { get; init; }
}