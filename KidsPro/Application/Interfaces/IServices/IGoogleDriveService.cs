namespace Application.Interfaces.IServices;

public interface IGoogleDriveService
{
   string UploadFilesToGoogleDrive(Stream videoStream);
}