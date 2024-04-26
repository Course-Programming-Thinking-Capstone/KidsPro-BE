using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface ICloudStorageService
{
    Task<string?> CreateFolderAndUpload(IFormFile fileVideo, string? videoName, string bucketName,
        string folderName);

    string GetVideoByName(string bucketName, string folderName, string videoName);
}