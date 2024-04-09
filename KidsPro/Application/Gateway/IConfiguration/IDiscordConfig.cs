namespace WebAPI.Gateway.IConfig;

public interface IDiscordConfig
{
    public string BotToken { get; } 
    public string ServerId { get; }
}