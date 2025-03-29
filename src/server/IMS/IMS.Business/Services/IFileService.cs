using System;
using Microsoft.AspNetCore.Http;

namespace IMS.Business.Services;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file);
    Task<bool> DeleteFileAsync(string fileUrl);
    // Task<string> UpdateFileAsync(IFormFile newFile, string oldFilePath, string subFolder);
}
