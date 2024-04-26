using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.IServices;

public interface IYoutubeV3Service
{
    Task<string?> UploadVideoToYoutube(IFormFile fileVideo, string? videoName);
}