namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) representing a user along with their assigned roles.
/// Used in administrative views or profile management.
/// </summary>
public class UserWithRolesDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public List<string> Roles { get; set; } = new();
}