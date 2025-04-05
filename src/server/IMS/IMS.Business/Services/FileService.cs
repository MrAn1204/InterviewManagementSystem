using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using IMS.Business.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

namespace IMS.Business.Services;

public class FileService : IFileService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public FileService(IConfiguration configuration)
    {
        var awsOptions = configuration.GetSection("AWS");
        _bucketName = awsOptions["BucketName"];

        _s3Client = new AmazonS3Client(
            awsOptions["AccessKey"],
            awsOptions["SecretKey"],
            RegionEndpoint.GetBySystemName(awsOptions["Region"])
        );
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

        var uploadRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey,
            InputStream = memoryStream, // Dùng memoryStream thay vì OpenReadStream()
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.Private // Tránh lỗi ACL nếu bucket không cho phép
        };

        try
        {
            await _s3Client.PutObjectAsync(uploadRequest);
            return $"https://{_bucketName}.s3.amazonaws.com/{fileKey}";
        }
        catch (AmazonS3Exception ex)
        {
            Console.WriteLine($"S3 error: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteFileAsync(string fileUrl)
    {
        if (string.IsNullOrEmpty(fileUrl))
            return false;

        // Lấy fileKey từ URL (bỏ phần domain)
        Uri uri = new Uri(fileUrl);
        string fileKey = uri.AbsolutePath.TrimStart('/');

        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey
        };

        await _s3Client.DeleteObjectAsync(deleteRequest);
        return true;
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
