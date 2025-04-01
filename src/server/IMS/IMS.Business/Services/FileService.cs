using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
}
