namespace App.DTO.v1.Identity;

public class UserFieldUpdate
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? Username { get; set; } = default!;
    public string Email { get; set; } = default!; // Required
}