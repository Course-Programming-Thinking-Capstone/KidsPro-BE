using Application.Dtos.Request;
using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/drives")]
public class DriveController : ControllerBase
{
    private IGoogleDriveService _driveService;

    public DriveController(IGoogleDriveService drive)
    {
        _driveService = drive;
    }

    /// <summary>
    /// Upload video to gg drive
    /// </summary>
    /// <param name="videoFile"></param>
    /// <param name="sectionId"></param>
    /// <returns></returns>
    [HttpPost("{sectionId:int}")]
    public async Task<ActionResult<string>> UploadVideoToDriveAsync(IFormFile videoFile, int sectionId)
    {
        var section = await _driveService.GetSectionInformationAsync(sectionId);

        //Create course folder
        var courseFolderId = _driveService.CreateCourseFolder(section.Course.Name);
        //Create section folder 
        var sectionFolderId = _driveService.CreateSectionFolder("Section: "+section.Name, courseFolderId);

        //Upload video file to gg drive
        var videoUrl = await _driveService
            .UploadVideoToGoogleDrive(videoFile,"Video: SectionId " +section.Id, sectionFolderId);

        return Ok(videoUrl);
    }
}