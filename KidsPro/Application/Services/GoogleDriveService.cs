using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Domain.Entities;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPI.Gateway.Configuration;
using WebAPI.Gateway.IConfig;

namespace Application.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private IDriveConfig _drive;
    private IUnitOfWork _unit;

    //khởi tạo phương thức google drive
    GoogleCredential _credential;
    private DriveService _service;
    private string KidsproFolderId = "1_m7ttcV-49Ct9rd2LuXN4ral1VZTxXw9";

    public GoogleDriveService(IDriveConfig drive, IUnitOfWork unit)
    {
        _drive = drive;
        _unit = unit;
    }

   
    private void InitializeGgDrive()
    {
        string jsonDrive = JsonConvert.SerializeObject(_drive);

        _credential = GoogleCredential.FromJson(jsonDrive).CreateScoped(new[]
        {
            DriveService.ScopeConstants.DriveFile
        });

        _service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = "Google Drive Upload Console App"
        });
    }

    public async Task<string?> UploadVideoToGoogleDrive(IFormFile fileVideo,string? videoName,string sectionFolderId)
    {
        // Tạo một luồng từ tệp đã tải lên
        using var stream = new MemoryStream();
        await fileVideo.CopyToAsync(stream);
        stream.Position = 0; // Đặt lại vị trí của con trỏ luồng
        
        // Tạo metadata cho file
        var fileMetadataVideo = new Google.Apis.Drive.v3.Data.File()
        {
            Name = videoName,
            Parents = new List<string> { sectionFolderId}
        };

        // Tạo yêu cầu tải file lên Google Drive
        var requestVideo = _service.Files.Create
            (fileMetadataVideo, stream, "video/*");
        requestVideo.Fields = "id";
        requestVideo.Upload();
        
        // Lấy ID của file đã tải lên
        var uploadedFileVideo = requestVideo.ResponseBody;
        var fileId = uploadedFileVideo.Id;
        
        // Cập nhật quyền chia sẻ file để công khai
        var permission = new Google.Apis.Drive.v3.Data.Permission()
        {
            Type = "anyone",
            Role = "reader"
        };
        await _service.Permissions.Create(permission, fileId).ExecuteAsync();
        // Tạo link preview
        var linkPreview = "https://drive.google.com/file/d/" + fileId + "/preview";
        return linkPreview;
    } 

    public string CreateParentFolder(string folderName)
    {
        InitializeGgDrive();

        var getCourseFolderIdExist = FindCourseFolderId(folderName);

        if (getCourseFolderIdExist == null)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { KidsproFolderId }
            };

            var request = _service.Files.Create(folderMetadata);
            request.Fields = "id";
            var createdFolder = request.Execute();
            return createdFolder.Id;
        }

        return getCourseFolderIdExist;
    }

    public string CreateChildFolder( string sectionFolderName, string courseFolderId)
    {
        InitializeGgDrive();

        var getSectionFolderIdExist = FindSectionFolderId(courseFolderId,sectionFolderName);
        if (getSectionFolderIdExist == null)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = sectionFolderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { courseFolderId }
            };

            var request = _service.Files.Create(folderMetadata);
            request.Fields = "id";
            var createdFolder = request.Execute();
            return createdFolder.Id;
        }

        return getSectionFolderIdExist;
    }

    private string? FindCourseFolderId(string folderName)
    {
        FilesResource.ListRequest listRequest = _service.Files.List();
        listRequest.Q = $"name = '{folderName}' and mimeType = 'application/vnd.google-apps.folder'";
        listRequest.Fields = "files(id)";

        FileList files = listRequest.Execute();
        if (files.Files.Count > 0)
            return files.Files[0].Id;
        return null;
    }

    private string? FindSectionFolderId(string courseFolderId, string sectionFolderName)
    {
        FilesResource.ListRequest listRequest = _service.Files.List();
        listRequest.Q =
            $"name = '{sectionFolderName}' and '{courseFolderId}' in parents and mimeType = 'application/vnd.google-apps.folder'";
        listRequest.Fields = "files(id)";

        FileList files = listRequest.Execute();
        if (files.Files.Count > 0)
            return files.Files[0].Id;
        return null;
    }
    public async Task<Section> GetSectionInformationAsync(int sectionId)
    {
        var section = await _unit.SectionRepository.GetByIdAsync(sectionId)
            ?? throw new BadRequestException($"SectionId {sectionId} not found");

        return section;
    }
}