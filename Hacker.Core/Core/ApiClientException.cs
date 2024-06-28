using System.Runtime.Serialization;

namespace Hacker.Core.Core;

[Serializable]
public class ApiClientException : AppException
{
    protected ApiClientException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    public ApiClientException() : base() { }
    public ApiClientException(string message) : base(message) { }
    public ApiClientException(string message, Exception? innerException) : base(message, innerException) { }

    public override ApiClientException AddData(string name, object? data)
    {
        base.AddData(name, data);
        return this;
    }
}
