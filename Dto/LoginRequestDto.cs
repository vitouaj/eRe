namespace ERE.DTO;

public class LoginRequestDto
{
    public string? EmailOrPhoneNumber { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string? DeviceId { get; set; }
    public string? DeviceName { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceOs { get; set; }
    public string? IPAddress { get; set; }
}