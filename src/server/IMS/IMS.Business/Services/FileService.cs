using Google.Cloud.Storage.V1;
using IMS.Business.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace IMS.Business.Services;

public class FileService : IFileService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public FileService(IConfiguration configuration)
    {
        _bucketName = configuration["GoogleCloudStorage:BucketName"]
            ?? throw new InvalidOperationException("GoogleCloudStorage:BucketName is not configured.");
        _storageClient = StorageClient.Create();
    }

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File không hợp lệ.");

        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string fileKey = $"cv/{fileName}";

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream); // Copy file vào memory stream
        memoryStream.Position = 0; // Reset vị trí đọc

        await _storageClient.UploadObjectAsync(_bucketName, fileKey, file.ContentType, memoryStream);
        return $"https://storage.googleapis.com/{_bucketName}/{fileKey}";
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return false;

        string fileKey = GetObjectKey(fileUrl);
        await _storageClient.DeleteObjectAsync(_bucketName, fileKey);
        return true;
    }

    private string GetObjectKey(string fileUrl)
    {
        if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri))
            return fileUrl.TrimStart('/');

        var path = Uri.UnescapeDataString(uri.AbsolutePath.TrimStart('/'));
        var bucketPrefix = $"{_bucketName}/";
        return path.StartsWith(bucketPrefix, StringComparison.OrdinalIgnoreCase)
            ? path[bucketPrefix.Length..]
            : path;
    }

    public async Task<byte[]> GenerateOfferExcelFile(List<OfferExcelDto> offerExcels)
    {
        // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        ExcelPackage.License.SetNonCommercialPersonal("My Name");

        using var package = new ExcelPackage();
        var worksheet = package.Workbook.Worksheets.Add("Offers list");

        // Ghi tiêu đề
        worksheet.Cells[1, 1].Value = "Candidate Name";
        worksheet.Cells[1, 2].Value = "Email";
        worksheet.Cells[1, 3].Value = "Approver";
        worksheet.Cells[1, 4].Value = "Department";
        worksheet.Cells[1, 5].Value = "Notes";
        worksheet.Cells[1, 6].Value = "Status";

        // Ghi dữ liệu từ offerExcels
        for (int i = 0; i < offerExcels.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = offerExcels[i].candidateName ?? "N/A";
            worksheet.Cells[i + 2, 2].Value = offerExcels[i].email ?? "N/A";
            worksheet.Cells[i + 2, 3].Value = offerExcels[i].approver ?? "N/A";
            worksheet.Cells[i + 2, 4].Value = offerExcels[i].department ?? "N/A";
            worksheet.Cells[i + 2, 5].Value = offerExcels[i].notes ?? "N/A";
            worksheet.Cells[i + 2, 6].Value = offerExcels[i].status ?? "N/A";
        }

        return package.GetAsByteArray();
    }
}
