using WebAPI.Gateway.IConfig;

namespace WebAPI.Gateway.Configuration;

public class ZaloPayConfig:IZaloPayConfig
{
    private readonly IConfigurationSection _section;
    public static string ConfigName => "ZaloPay";
    public string AppId =>  _section[nameof(AppId)];
    public string ReturnUrl => _section[nameof(ReturnUrl)];
    public string IpnUrl  => _section[nameof(IpnUrl)];
    public string Key1  => _section[nameof(Key1)];
    public string Key2 => _section[nameof(Key2)];
    public string PaymentUrl  => _section[nameof(PaymentUrl)];

    public ZaloPayConfig(IConfiguration configuration)
    {
        _section = configuration.GetSection(ConfigName);
    }
}