namespace Application.Interfaces.IServices;

public interface IDiscordService
{
    Task<string> CreateVoiceChannelAsync(string voiceChannelName);
}