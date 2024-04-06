using Microsoft.Extensions.Configuration;
using WebAPI.Gateway.IConfig;

namespace WebAPI.Gateway.Configuration;

public class DiscordConfig:IDiscordConfig
{
    private readonly IConfigurationSection _section;

    public static string ConfigName => "Discord";
    public string BotToken => _section[nameof(BotToken)];
    public string ServerId => _section[nameof(ServerId)];
   

    public DiscordConfig(IConfiguration configuration)
    {
        _section = configuration.GetSection(ConfigName);
    }
}