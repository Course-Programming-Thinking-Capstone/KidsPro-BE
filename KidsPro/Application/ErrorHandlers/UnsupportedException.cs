using System.Net;

namespace Application.ErrorHandlers;

public class UnsupportedException : BaseException
{
    private const int _statusCode = (int)HttpStatusCode.UnsupportedMediaType;
    private const string? _title = "Unsupported.";

    public UnsupportedException()
    {
        StatusCode = _statusCode;
        Title = _title;
    }

    public UnsupportedException(string? message) : base(message)
    {
        StatusCode = _statusCode;
        Title = _title;
    }
}