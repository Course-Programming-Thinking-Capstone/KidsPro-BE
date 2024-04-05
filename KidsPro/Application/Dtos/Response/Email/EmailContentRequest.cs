namespace Application.Dtos.Request.Email;

public class EmailContentRequest
{
    public string? StudentName { get; set; }
    public string? Birthday { get; set; }
    public string? Account { get; set; }
    public string? Password { get; set; }
    public string? Note { get; set; }
    public string? Email { get; set; }
    public int ParentId { get; set; }
}