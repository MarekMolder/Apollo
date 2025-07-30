namespace App.DTO.v1.Identity;

/// <summary>
/// Data Transfer Object (DTO) for assigning a role to a user.
/// Used in administrative actions related to user-role management.
/// </summary>
public class AssignRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}