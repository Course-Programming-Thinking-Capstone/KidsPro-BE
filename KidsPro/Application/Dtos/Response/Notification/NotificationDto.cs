namespace Application.Dtos.Response.Notification;

public class NotificationDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string Content { get; set; } = null!;

    public string Date { get; set; } = null!;

    public bool IsRead { get; set; }
}