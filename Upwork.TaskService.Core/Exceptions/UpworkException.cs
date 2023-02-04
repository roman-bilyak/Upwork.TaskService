using System.Runtime.Serialization;

namespace Upwork.TaskService;

public class UpworkException : Exception
{
    public UpworkException()
        : base()
    {
    }

    public UpworkException(string? message)
        : base(message)
    {
    }

    public UpworkException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected UpworkException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
