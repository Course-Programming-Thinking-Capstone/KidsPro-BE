using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3;
using Newtonsoft.Json;
using WebAPI.Gateway.Configuration;
using WebAPI.Gateway.IConfig;

namespace Application.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private IDriveConfig _drive;
    private string _folderId = "1_m7ttcV-49Ct9rd2LuXN4ral1VZTxXw9";

    public GoogleDriveService(IDriveConfig drive)
    {
        _drive = drive;
    }

    public string UploadFilesToGoogleDrive(Stream videoStream)
    {
        
        string jsonDrive = JsonConvert.SerializeObject(_drive);
        
        //khởi tạo phương thức google drive
        GoogleCredential credential;
        // {
        credential = GoogleCredential.FromJson(jsonDrive).CreateScoped(new[]
        {
            DriveService.ScopeConstants.DriveFile
        });


        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "Google Drive Upload Console App"
        });

        // Tạo thư mục "Course"
        var courseFolder = CreateFolder(service, "Course35", _folderId);

        // Tạo thư mục "Section" trong thư mục "Course"
        var sectionFolder = CreateFolder(service, "Section", courseFolder.Id);

        #region Video

        // Tạo metadata cho file
        var fileMetadataVideo = new Google.Apis.Drive.v3.Data.File()
        {
            Name = "Tên video",
            Parents = new List<string> { sectionFolder.Id }
        };

        // Tạo yêu cầu tải file lên Google Drive
        var requestImage = service.Files.Create
            (fileMetadataVideo, videoStream, "video/*");
        requestImage.Fields = "id, webViewLink";

        // Thực hiện yêu cầu
        requestImage.Upload();
        // Lấy thông tin file đã tải lên
        var uploadedFileVideo = requestImage.ResponseBody;
        return uploadedFileVideo.WebViewLink;

        #endregion
    } //end function

    private Google.Apis.Drive.v3.Data.File CreateFolder(DriveService service, string folderName, string parentFolderId)
    {
        var folderMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = folderName,
            MimeType = "application/vnd.google-apps.folder",
            Parents = new List<string> { parentFolderId }
        };

        var request = service.Files.Create(folderMetadata);
        request.Fields = "id";
        return request.Execute();
    }
}