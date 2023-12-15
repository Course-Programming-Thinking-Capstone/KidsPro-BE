namespace Application.Configurations;

public class AppConfiguration
{
    public string? DatabaseConnection { get; set; }
    public string? Key { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }

    public AppConfiguration()
    {
    }
}