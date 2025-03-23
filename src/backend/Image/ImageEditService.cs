using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;

namespace AS_2025.Image;

public class ImageEditService
{
    private const string Font = "Arial.ttf";
    private const int FontSize = 12;
    private static readonly Color Color = Color.Red;

    public async Task<ReadOnlyMemory<byte>> DrawTextAsync(ReadOnlyMemory<byte> content, int x, int y, string text, CancellationToken cancellationToken)
    {
        using var imageStream = new MemoryStream(content.ToArray());
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(imageStream, cancellationToken);

        var fontCollection = new FontCollection();
        var fontFamily = fontCollection.Add(Font);
        var font = fontFamily.CreateFont(FontSize, FontStyle.Regular);

        image.Mutate(context => context.DrawText(text, font, Color, new PointF(x, y)));

        var outputStream = new MemoryStream();
        await image.SaveAsJpegAsync(outputStream, cancellationToken: cancellationToken);
        outputStream.Position = 0;

        return outputStream.ToArray();
    }
}