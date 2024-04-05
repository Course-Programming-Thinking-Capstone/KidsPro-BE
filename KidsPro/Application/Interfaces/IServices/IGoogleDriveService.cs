namespace Application.Interfaces.IServices;

public interface IGoogleDriveService
{
   string UploadFilesToGoogleDrive(string credentialsPath, string kidsProFolder, Stream fileStream);
}