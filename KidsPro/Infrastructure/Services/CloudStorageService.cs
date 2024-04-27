using System.Text;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPI.Gateway.IConfig;

namespace Infrastructure.Services;

public class CloudStorageService : ICloudStorageService
{
    private IDriveConfig _drive;

    //khởi tạo phương thức google drive
    GoogleCredential _credential;
    private StorageClient _storage;

    public CloudStorageService(IDriveConfig drive)
    {
        _drive = drive;
    }

    private void InitializeGgStorage()
    {
        string jsonStorage = JsonConvert.SerializeObject(_drive);

        _credential = GoogleCredential.FromJson(jsonStorage).CreateScoped(new[]
        {
            StorageService.ScopeConstants.DevstorageFullControl
        });

        _storage = StorageClient.Create(_credential);
    }

    private async Task<string?> UploadVideoToCloudStorage(IFormFile fileVideo, string? videoName, string bucketName,
        string folderName)
    {
        using var stream = new MemoryStream();
        await fileVideo.CopyToAsync(stream);
        stream.Position = 0; // Đặt lại vị trí của con trỏ luồng

        using var fileStream = stream;
        var objectName = $"{folderName}/{videoName ?? fileVideo.FileName}";
        var contentType = "video/mp4";
        var obj = new Google.Apis.Storage.v1.Data.Object()
        {
            Bucket = bucketName,
            Name = objectName,
            ContentType = contentType,
        };
        await _storage.UploadObjectAsync(obj, fileStream);
        // Trả về URL SignedURL
        return GenerateV4SignedReadUrl(bucketName, objectName);
    }

    private string GenerateV4SignedReadUrl(string bucketName, string objectName)
    {
        UrlSigner urlSigner = UrlSigner.FromCredential(_credential);
        string url = urlSigner.Sign(bucketName, objectName, TimeSpan.FromSeconds(30), HttpMethod.Get);
        return url;
    }

    public async Task<string?> CreateFolderAndUpload(IFormFile fileVideo, string? videoName, string bucketName,
        string folderName)
    {
        InitializeGgStorage();

        var folderObject = $"{folderName}/";
        try
        {
            var folder = _storage.GetObject(bucketName, folderObject);
        }
        catch (GoogleApiException e) when (e.Error.Code == 404)
        {
            // "Thư mục" chưa tồn tại, tạo mới
            var content = Encoding.UTF8.GetBytes("");
            using (var stream = new MemoryStream(content))
            {
                _storage.UploadObject(bucketName, folderObject, "application/x-directory", stream);
            }
        }
        return await UploadVideoToCloudStorage(fileVideo, videoName, bucketName, folderName);

    }
    
    public string GetVideoByName(string bucketName, string folderName, string videoName)
    {
        InitializeGgStorage();
        var objectName = $"{folderName}/{videoName}";
        try
        {
            var video = _storage.GetObject(bucketName, objectName);
        }
        catch (GoogleApiException e) 
        {
            throw new NotFoundException("Video not found");
        }
         return GenerateV4SignedReadUrl(bucketName, objectName);
    }
    
}