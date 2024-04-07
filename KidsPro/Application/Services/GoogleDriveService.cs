using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Drive.v3;

namespace Application.Services;

public class GoogleDriveService : IGoogleDriveService
{
    public string UploadFilesToGoogleDrive(string credentialsPath, string kidsProFolder, Stream fileStream)
    {
        //khởi tạo phương thức google drive
        GoogleCredential credential;
        using (var stream = new FileStream(credentialsPath, FileMode.Open, FileAccess.Read))
        {
            credential = GoogleCredential.FromStream(stream).CreateScoped(new[]
            {
                DriveService.ScopeConstants.DriveFile
            });
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Google Drive Upload Console App"
            });

            // Tạo thư mục "Course"
            var courseFolder = CreateFolder(service, "Course", kidsProFolder);
            Console.WriteLine("Created Course folder with ID: " + courseFolder.Id);

            // Tạo thư mục "Section" trong thư mục "Course"
            var sectionFolder = CreateFolder(service, "Section", courseFolder.Id);
            Console.WriteLine("Created Section folder with ID: " + sectionFolder.Id);

            #region Video

            // Tạo metadata cho file
            var fileMetadataVideo = new Google.Apis.Drive.v3.Data.File()
            {
                Name = "Tên video",
                Parents = new List<string> { sectionFolder.Id }
            };

            // Tạo yêu cầu tải file lên Google Drive
            var requestImage = service.Files.Create
                (fileMetadataVideo, fileStream, "video/*");
            requestImage.Fields = "id, webViewLink";

            // Thực hiện yêu cầu
            requestImage.Upload();
            // Lấy thông tin file đã tải lên
            var uploadedFileVideo = requestImage.ResponseBody;

            // In ra ID và URL của file
            Console.WriteLine($"Video uploaded with ID: {uploadedFileVideo.Id}");
            return uploadedFileVideo.WebViewLink;

            #endregion
        } //end using

        throw new BadRequestException("File not found");
    } //end function

    public Google.Apis.Drive.v3.Data.File CreateFolder(DriveService service, string folderName, string parentFolderId)
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