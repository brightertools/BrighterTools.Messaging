namespace BrighterTools.Messaging.Postmark.Services;

/// <summary>A documented provider rejection, not a timeout or an unknown send outcome.</summary>
public sealed class PostmarkRejectedException(int errorCode) : InvalidOperationException("Postmark rejected the message.")
{
    /// <summary>Non-sensitive numeric provider error code.</summary>
    public int ErrorCode { get; } = errorCode;
}
