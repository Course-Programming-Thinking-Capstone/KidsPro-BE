namespace WebAPI.Gateway.IConfig;

public interface IZaloPayConfig
{
    public string AppId { get; } 
    public string ReturnUrl { get; }
    public string IpnUrl { get; }
    public string Key1 { get; }
    public string Key2 { get; }
    public string PaymentUrl { get; }
}