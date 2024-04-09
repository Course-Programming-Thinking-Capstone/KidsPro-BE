using Application.Interfaces.IServices;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/v1/drives")]
public class DriveController : ControllerBase
{
    private IGoogleDriveService _drive;
    public DriveController(IGoogleDriveService drive)
    {
        _drive = drive;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // Tạo một luồng từ tệp đã tải lên
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0; // Đặt lại vị trí của con trỏ luồng
    
        var result=_drive.UploadFilesToGoogleDrive(stream);
        return Ok(new
        {
            Url=result
        });
    }


}