using Application.Configurations;
using Application.Dtos.Request;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/drives")]
public class DrivesController : ControllerBase
{
    private IGoogleDriveService _driveService;
    private ICloudStorageService _cloudStorage;

    public DrivesController(IGoogleDriveService drive, ICloudStorageService youtubeV3Service)
    {
        _driveService = drive;
        _cloudStorage = youtubeV3Service;
    }

    /// <summary>
    /// Upload video to Google Drive
    /// </summary>
    /// <param name="videoFile"></param>
    /// <param name="sectionId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    //[Authorize(Roles = $"{Constant.TeacherRole},{Constant.AdminRole}")]
    [HttpPost("drive")]
    public async Task<ActionResult<string>> UploadVideoToDriveAsync(IFormFile? videoFile, int sectionId,int index)
    {
        if (videoFile == null) throw new BadRequestException("Video file is empty");
        
        var section = await _driveService.GetSectionInformationAsync(sectionId);

        //Create course folder
        var courseFolderId = _driveService.CreateParentFolder("Course: " + section.Course.Name);
        //Create lesson folder 
        var sectionFolderId = _driveService.CreateChildFolder("Section: "+section.Id, courseFolderId);
        //Create video  folder 
        var lessonFolderId = _driveService.CreateChildFolder("Lesson: "+index, sectionFolderId);

        //Upload video file to gg drive
        var videoUrl = await _driveService
            .UploadVideoToGoogleDrive(videoFile,"Lesson "+index+" - Video", lessonFolderId);

        return Ok(videoUrl);
    }

    /// <summary>
    /// Upload video to Google Cloud Storage
    /// </summary>
    /// <param name="videoFile"></param>
    /// <param name="videoName"></param>
    /// <param name="sectionId"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.TeacherRole},{Constant.AdminRole}")]
    [HttpPost("cloud")]
    public async Task<ActionResult<string>> UploadVideoToStorageAsync(IFormFile? videoFile,string? videoName,int sectionId)
    {
        if (videoFile == null) throw new BadRequestException("Video file is empty");
        
        var section = await _driveService.GetSectionInformationAsync(sectionId);
        var bucket = "kidspro";
        var folderName =   section.Course.Name;
        var nameOfVideo = "Section"+sectionId+" "+videoName;
        //Upload video file to gg drive
        var videoUrl = await _cloudStorage.CreateFolderAndUpload(videoFile,nameOfVideo,bucket,folderName);

        return Ok(videoUrl);
    }
    /// <summary>
    /// Get video link from cloud storage
    /// </summary>
    /// <param name="videoName"></param>
    /// <param name="sectionId"></param>
    /// <returns></returns>
    [HttpGet("cloud")]
    public async Task<IActionResult> GetVideoToStorageAsync(string videoName,int sectionId)
    {
        
        var section = await _driveService.GetSectionInformationAsync(sectionId);
        var bucket = "kidspro";
        var folderName = section.Course.Name;
        var nameOfVideo = "Section"+sectionId+" "+videoName;
        //Upload video file to gg drive
        var videoUrl =  _cloudStorage.GetVideoByName(bucket,folderName,nameOfVideo);

        return Ok(videoUrl);
    }
}