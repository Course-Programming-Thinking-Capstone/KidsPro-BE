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
    private IYoutubeV3Service _youtubeV3Service;

    public DrivesController(IGoogleDriveService drive, IYoutubeV3Service youtubeV3Service)
    {
        _driveService = drive;
        _youtubeV3Service = youtubeV3Service;
    }

    /// <summary>
    /// Upload video to gg drive
    /// </summary>
    /// <param name="videoFile"></param>
    /// <param name="sectionId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
   // [Authorize(Roles = $"{Constant.TeacherRole},{Constant.AdminRole}")]
    [HttpPost("drive")]
    public async Task<ActionResult<string>> UploadVideoToDriveAsync(IFormFile? videoFile, int sectionId,int index)
    {
        if (videoFile == null) throw new BadRequestException("Video file is empty");
        
        var section = await _driveService.GetSectionInformationAsync(sectionId);

        //Create course folder
        var courseFolderId = _driveService.CreateParentFolder("Course: "+section.Course.Name);
        //Create lesson folder 
        var sectionFolderId = _driveService.CreateChildFolder("Section: "+section.Id, courseFolderId);
        //Create video  folder 
        var lessonFolderId = _driveService.CreateChildFolder("Lesson: "+index, sectionFolderId);

        //Upload video file to gg drive
        var videoUrl = await _driveService
            .UploadVideoToGoogleDrive(videoFile,"Lesson "+index+" - Video", lessonFolderId);

        return Ok(videoUrl);
    }
    
    [HttpPost("youtube")]
    public async Task<ActionResult<string>> UploadVideoToYoutubeAsync(IFormFile? videoFile)
    {
        if (videoFile == null) throw new BadRequestException("Video file is empty");
        
        //Upload video file to gg drive
        var videoUrl = await _youtubeV3Service.UploadVideoToYoutube(videoFile, "Video Test");

        return Ok(videoUrl);
    }
}