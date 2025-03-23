namespace AS_2025.Minio;

public record GetFileInfo(string FileName, ReadOnlyMemory<byte> Content, string ContentType);