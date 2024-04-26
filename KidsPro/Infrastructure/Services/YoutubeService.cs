using Application;
using Application.Interfaces.IServices;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebAPI.Gateway.IConfig;


public class YoutubeV3Service:IYoutubeV3Service
{
    private IYoutubeConfig _youtube;
    private IUnitOfWork _unit;

    //khởi tạo phương thức google drive
    GoogleCredential _credential;
    YouTubeService _youtubeService;

    public YoutubeV3Service(IYoutubeConfig youtube, IUnitOfWork unit)
    {
        _youtube = youtube;
        _unit = unit;
    }

    private async Task InitializeYoutube()
    {
        string jsonYoutube = JsonConvert.SerializeObject(_youtube);
        
        _credential = GoogleCredential.FromJson(jsonYoutube).CreateScoped(new[]
        {
            YouTubeService.ScopeConstants.Youtube
        });

        _youtubeService = new YouTubeService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = _credential,
            ApplicationName = "YouTube Upload Console App"
        });
    }

    public async Task<string?> UploadVideoToYoutube(IFormFile fileVideo, string? videoName)
    {
        await InitializeYoutube();
        // Tạo một luồng từ tệp đã tải lên
        using var stream = new MemoryStream();
        await fileVideo.CopyToAsync(stream);
        stream.Position = 0; // Đặt lại vị trí của con trỏ luồng

        // Tạo metadata cho video
        var video = new Video();
        video.Snippet = new VideoSnippet();
        video.Snippet.Title = videoName;
        video.Snippet.Description = "This is a description of the video.";
        video.Snippet.Tags = new string[] { "tag1", "tag2" };
        video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
        video.Status = new VideoStatus();
        video.Status.PrivacyStatus = "unlisted"; // or "private" or "public"

        // Tạo yêu cầu tải lên
        var videosInsertRequest = _youtubeService.Videos.Insert(video, "snippet,status", stream, "video/*");
        videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
        videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;
        try
        {
           var test= await videosInsertRequest.UploadAsync();
           return test.Exception.Message + " " + test.Status + " "+ test.ToString();
        }catch (Google.GoogleApiException ex)
        {
           return ($"An error occurred: {ex.Message}");
        }

        // if (!string.IsNullOrEmpty(videoId))
        // {
        //     // Tạo liên kết video
        //     string videoLink = "https://www.youtube.com/watch?v=" + videoId;
        //     return videoLink;
        // }
        return null; 
    }
    void videosInsertRequest_ProgressChanged(IUploadProgress progress)
    {
        switch (progress.Status)
        {
            case UploadStatus.Uploading:
                Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                break;

            case UploadStatus.Failed:
                Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                break;
        }
    }

    void videosInsertRequest_ResponseReceived(Video video)
    {
        Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
    }

}