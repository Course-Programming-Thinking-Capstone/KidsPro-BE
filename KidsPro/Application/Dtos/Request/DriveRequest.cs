using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Request;

public class DriveRequest
{
    public string? NameVideo { get; set; }
    public string? NameCourse { get; set; }
    public string? NameSection { get; set; }
}