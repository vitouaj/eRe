namespace ERE.DTO;
public class Response
{
    public Response(bool success, object payload, string message)
    {
        Success = success;
        Payload = payload;
        Message = message;
    }
    public Response() { }
    public bool? Success { get; set; }
    public object? Payload { get; set; }
    public string? Message { get; set; }
}
