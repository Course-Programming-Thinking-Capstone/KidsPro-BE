using Application.ErrorHandlers;
using Application.Interfaces.IServices;
using Discord;
using Discord.WebSocket;
using WebAPI.Gateway.IConfig;

namespace Infrastructure.Services;

public class DiscordService:IDiscordService
{
    private IDiscordConfig _discord;
    //Discord
    private static DiscordSocketClient _client;

    public DiscordService(IDiscordConfig discord)
    {
        _discord = discord;
    }

    #region Discord

    private async Task InitializeDiscordConnection()
    {
        try
        {
            _client = new DiscordSocketClient();
            await _client.LoginAsync(TokenType.Bot, _discord.BotToken);
            await _client.StartAsync();
            // Kiểm tra xem bot đã kết nối thành công
            if (_client.ConnectionState == ConnectionState.Connected) return;
            // Đợi bot kết nối và sẵn sàng hoạt động, Chờ 5 giây
            await Task.Delay(5000);
        }
        catch (Exception ex)
        {
            throw new BadRequestException("Unable connect to Discord Bot, error " + ex);
        }
    }

    public async Task<string> CreateVoiceChannelAsync(string voiceChannelName)
    {
        //Connect to discord
        await InitializeDiscordConnection();

        var guild = _client.GetGuild(ulong.Parse(_discord.ServerId))
                    ?? throw new NotFoundException("Discord server Id not found");

        var newVoiceChannel = await guild.CreateVoiceChannelAsync(voiceChannelName);
        return $"https://discord.com/channels/{_discord.ServerId}/{newVoiceChannel.Id}";
    }

    #endregion
}