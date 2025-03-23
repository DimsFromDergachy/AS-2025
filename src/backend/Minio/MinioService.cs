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

    public async Task<UploadFileInfo> UploadAsync(string fileName, ReadOnlyMemory<byte> content, string contentType, CancellationToken cancellationToken)
    {
        var client = _minioClientFactory.CreateClient();

        await CheckBucket(client, cancellationToken);

        await using var stream = content.AsStream();

        var objectId = $"{Guid.NewGuid()}-{fileName}";
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_options.Value.Bucket)
            .WithObject(objectId)
            .WithStreamData(stream)
            .WithObjectSize(stream.Length).WithContentType(contentType);
        await client.PutObjectAsync(putObjectArgs, cancellationToken);

        var presignedGetObjectArgs = new PresignedGetObjectArgs()
            .WithBucket(_options.Value.Bucket)
            .WithObject(objectId)
            .WithExpiry(Expiry);

        var url = await client.PresignedGetObjectAsync(presignedGetObjectArgs);
        return new UploadFileInfo(objectId, url);
    }

    public async Task<GetFileInfo> GetAsync(string objectId, CancellationToken cancellationToken)
    {
        var client = _minioClientFactory.CreateClient();

        await CheckBucket(client, cancellationToken);

        var memoryStream = new MemoryStream();
        try
        {
            var getObjectArgs = new GetObjectArgs()
                .WithBucket(_options.Value.Bucket)
                .WithObject(objectId)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            var objectStat = await client.GetObjectAsync(getObjectArgs, cancellationToken);

            return new GetFileInfo(objectStat.ObjectName, memoryStream.ToArray(), objectStat.ContentType);
        }
        finally
        {
            await memoryStream.DisposeAsync();
        }
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