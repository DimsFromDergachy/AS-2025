namespace AS_2025.Api.Department.Export;

public record ExportDepartmentsResponse(byte[] Bytes, string ContentType, string FileName);