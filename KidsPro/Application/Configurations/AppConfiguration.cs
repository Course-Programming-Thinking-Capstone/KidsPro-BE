namespace Application.Configurations;

public class AppConfiguration
{
    public string DatabaseConnection { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;

    public AppConfiguration()
    {
    }

    public AppConfiguration(string databaseConnection, string key, string issuer, string audience)
    {
        DatabaseConnection = databaseConnection;
        Key = key;
        Issuer = issuer;
        Audience = audience;
    }
}