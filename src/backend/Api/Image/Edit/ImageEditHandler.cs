using Ardalis.Result;
using AS_2025.Image;
using AS_2025.Minio;
using MediatR;

namespace AS_2025.Api.Image.Edit;

public class ImageEditHandler : IRequestHandler<ImageEditRequest, Result<ImageEditResponse>>
{
    private readonly MinioService _minioService;
    private readonly ImageEditService _imageEditService;

    public ImageEditHandler(MinioService minioService, ImageEditService imageEditService)
    {
        _minioService = minioService;
        _imageEditService = imageEditService;
    }

    public async Task<Result<ImageEditResponse>> Handle(ImageEditRequest request, CancellationToken cancellationToken)
    {
        await using var stream = request.File.OpenReadStream();
        
        using var target = new MemoryStream();
        await stream.CopyToAsync(target, cancellationToken);
        target.Position = 0;

        var originalFileInfo = await _minioService.UploadAsync(request.File.FileName, target.ToArray(), request.File.ContentType, cancellationToken);

        var modifiedContent = await _imageEditService.DrawTextAsync(target.ToArray(), request.X, request.Y, request.Text, cancellationToken);
        var modifiedFileInfo = await _minioService.UploadAsync($"{request.File.FileName}.modified", modifiedContent, request.File.ContentType, cancellationToken);

        return new Result<ImageEditResponse>(new ImageEditResponse(originalFileInfo.ObjectId, originalFileInfo.Url, modifiedFileInfo.ObjectId, modifiedFileInfo.Url));
    }
}