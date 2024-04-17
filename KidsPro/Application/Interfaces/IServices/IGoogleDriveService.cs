using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface IGoogleDriveService
{
    string CreateParentFolder(string parentFolderName);
    string CreateChildFolder(string childFolderName, string parentFolderId);
    Task<string?> UploadVideoToGoogleDrive(IFormFile fileVideo,string? videoName,string videoFolderId);
    Task<Section> GetSectionInformationAsync(int sectionId);
}