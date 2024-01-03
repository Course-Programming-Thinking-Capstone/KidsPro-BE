using System.Net;
using Application.Configurations;
using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Firebase.Auth;
using Firebase.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class FireBaseImageService : IImageService
{
    private readonly AppConfiguration _configuration;

    private readonly ILogger<IImageService> _logger;

    private static readonly List<string> SUPPORTED_IMAGE_FILE = new()
    {
        ".jpg",
        ".jpeg",
        ".png",
        ".svg",
        ".webp",
        ".heic",
        ".heif",
        ".ico",
        ".gif"
    };

    public FireBaseImageService(AppConfiguration configuration, ILogger<IImageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> UploadImage(IFormFile file, string folderPath, string fileName)
    {
        var stream = file.OpenReadStream();

        var fileExtension = Path.GetExtension(file.FileName);

        //Check file extension for preventing malware
        if (!SUPPORTED_IMAGE_FILE.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Unsupported file type.");
        }

        fileName += fileExtension;

        var firebaseAuthLink = await GetAuthentication();

        // you can use CancellationTokenSource to cancel the upload midway
        var cancellation = new CancellationTokenSource();

        var task = new FirebaseStorage(
                _configuration.FireBaseBucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken),
                    ThrowOnCancel =
                        true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
            .Child(folderPath)
            .Child(fileName)
            .PutAsync(stream, cancellation.Token);

        task.Progress.ProgressChanged += (s, e) => Console.WriteLine($"Progress: {e.Percentage} %");

        try
        {
            // cancel the upload
            // cancellation.Cancel();

            return await task;
        }
        catch (Exception e)
        {
            _logger.LogError("Error when upload file to firebase. Detail:\n {}", e.Message);
            throw new Exception("Error when upload file to firebase.");
        }
    }

    public async Task RemoveFile(string fileUrl)
    {
        var firebaseAuthLink = await GetAuthentication();

        var storage = new FirebaseStorage(
            _configuration.FireBaseBucket,
            new FirebaseStorageOptions
            {
                AuthTokenAsyncFactory = () => Task.FromResult(firebaseAuthLink.FirebaseToken)
            });

        // Extract the encoded file path from the Firebase Storage URL
        var uri = new Uri(fileUrl);
        var encodedFilePath = uri.Segments.Last();

        // URL decode the file path
        var decodedFilePath = WebUtility.UrlDecode(encodedFilePath);

        try
        {
            // Remove the file based on its path
            await storage.Child(decodedFilePath).DeleteAsync();
        }
        catch (Exception e)
        {
            throw new BadRequestException($"Error when remove file: \n{e.Message}");
        }
    }

    private async Task<FirebaseAuthLink> GetAuthentication()
    {
        var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.FireBaseApiKey));
        return await auth.SignInWithEmailAndPasswordAsync(_configuration.FireBaseAuthEmail,
            _configuration.FireBaseAuthPassword);
    }
}