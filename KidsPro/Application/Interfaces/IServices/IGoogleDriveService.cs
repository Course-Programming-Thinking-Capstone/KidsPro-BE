using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface IGoogleDriveService
{
    string CreateCourseFolder(string folderName);
    string CreateSectionFolder(string sectionFolderName, string courseFolderId);
    Task<string?> UploadVideoToGoogleDrive(IFormFile fileVideo,string? videoName,string sectionFolderId);
    Task<Section> GetSectionInformationAsync(int sectionId);
}