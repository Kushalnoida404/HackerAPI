using System.Runtime.Serialization;

namespace Hacker.Core.Core;

[Serializable]
public class NameValueList<T> : Dictionary<string, T>
{
    public NameValueList() : base(StringComparer.OrdinalIgnoreCase)
    {
    }

    #pragma warning disable SYSLIB0051 // Type or member is obsolete
    protected NameValueList(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
    #pragma warning restore SYSLIB0051 // Type or member is obsolete
    {
    }
}