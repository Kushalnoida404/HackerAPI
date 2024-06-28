using System.Runtime.Serialization;
namespace Hacker.Core.Core;

[Serializable]
public class AppException : Exception
{
#pragma warning disable SYSLIB0051 // Type or member is obsolete
    protected AppException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#pragma warning restore SYSLIB0051 // Type or member is obsolete
    public AppException() : base() { }
    public AppException(string message) : base(message) { }
    public AppException(string message, Exception? innerException) : base(message, innerException) { }

    public virtual AppException AddData(string name, object? data)
    {
        this.Data.Add(name, data);
        return this;
    }
}
