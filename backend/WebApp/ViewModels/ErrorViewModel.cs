namespace WebApp.ViewModels;

/// <summary>
/// ViewModel for representing error information to the user.
/// </summary>
public class ErrorViewModel
{
    /// <summary>
    /// The unique identifier for the current request.
    /// Useful for tracing errors in logs.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Determines whether the RequestId should be shown in the UI.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}