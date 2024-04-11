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

    public DrivesController(IGoogleDriveService drive)
    {
        _driveService = drive;
    }

    /// <summary>
    /// Upload video to gg drive
    /// </summary>
    /// <param name="videoFile"></param>
    /// <param name="sectionId"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    [Authorize(Roles = $"{Constant.TeacherRole},{Constant.AdminRole}")]
    [HttpPost]
    public async Task<ActionResult<string>> UploadVideoToDriveAsync(IFormFile? videoFile, int sectionId,int index)
    {
        if (videoFile == null) throw new BadRequestException("Video file is empty");
        
        var section = await _driveService.GetSectionInformationAsync(sectionId);

        //Create course folder
        var courseFolderId = _driveService.CreateParentFolder("Course: "+section.Course.Name);
        //Create lesson folder 
        var lessonFolderId = _driveService.CreateChildFolder("Lesson: "+section.Id, courseFolderId);
        //Create video  folder 
        var videoFolderId = _driveService.CreateChildFolder("Video: "+index, lessonFolderId);

        //Upload video file to gg drive
        var videoUrl = await _driveService
            .UploadVideoToGoogleDrive(videoFile,"Lesson "+section.Id+" - Video " +index, videoFolderId);

        return Ok(videoUrl);
    }
}