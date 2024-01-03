using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface IImageService
{
    Task<string> UploadImage(IFormFile file, string folderPath, string fileName);

    Task RemoveFile(string filePath);
}