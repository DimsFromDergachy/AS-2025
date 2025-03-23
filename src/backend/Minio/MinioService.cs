using AS_2025.Options;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Options;
using Minio.DataModel.Args;
using Minio;

namespace AS_2025.Minio;

public class MinioService
{
    private const int Expiry = 60 * 60 * 24; // a day

    private readonly IOptions<MinioOptions> _options;
    private readonly IMinioClientFactory _minioClientFactory;

    public MinioService(IMinioClientFactory minioClientFactory, IOptions<MinioOptions> options)
    {
        _minioClientFactory = minioClientFactory;
        _options = options;
    }

    public async Task<string> UploadAsync(string fileName, ReadOnlyMemory<byte> content, string contentType, CancellationToken cancellationToken)
    {
        var client = _minioClientFactory.CreateClient();

        await CheckBucket(client, cancellationToken);

        await using var stream = content.AsStream();

        var objectName = $"{Guid.NewGuid()}-{fileName}";
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.Value.Bucket)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length).WithContentType(contentType);
        await client.PutObjectAsync(putObjectArgs, cancellationToken);

        var presignedGetObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(_options.Value.Bucket)
            .WithObject(objectName)
            .WithExpiry(Expiry);

        return await client.PresignedGetObjectAsync(presignedGetObjectArgs);
    }

    private async Task CheckBucket(IMinioClient minioClient, CancellationToken cancellationToken)
    {
        var bucketExistsArgs = new BucketExistsArgs().WithBucket(_options.Value.Bucket);
        var found = await minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(_options.Value.Bucket);
            await minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }
    }
}