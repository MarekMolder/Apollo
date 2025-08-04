namespace App.DTO.v1;

/// <summary>
/// Generic response DTO for returning one or more messages (e.g. success, validation, or error messages).
/// </summary>
public class Message
{
    public Message()
    {
    }

    public Message(params string[] messages)
    {
        Messages = messages;
    }

    public ICollection<string> Messages { get; set; } = new List<string>();
}