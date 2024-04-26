using Microsoft.Extensions.Configuration;
using WebAPI.Gateway.IConfig;

namespace WebAPI.Gateway.Configuration;

public class YoutubeConfig:IYoutubeConfig
{
    private readonly IConfigurationSection _section;
    public static string ConfigName => "Youtube";
    public string type => _section[nameof(type)];
    public string project_id => _section[nameof(project_id)];
    public string private_key_id => _section[nameof(private_key_id)];
    public string client_email => _section[nameof(client_email)];
    public string client_id => _section[nameof(client_id)];
    public string auth_uri => _section[nameof(auth_uri)];
    public string token_uri => _section[nameof(token_uri)];
    public string private_key => _section[nameof(private_key)];
    public string auth_provider_x509_cert_url => _section[nameof(auth_provider_x509_cert_url)];
    public string client_x509_cert_url => _section[nameof(client_x509_cert_url)];
    public string universe_domain => _section[nameof(universe_domain)];
    public string client_secret=> _section[nameof(client_secret)];
   
    public YoutubeConfig(IConfiguration configuration)
    {
        _section = configuration.GetSection(ConfigName);
    }
}