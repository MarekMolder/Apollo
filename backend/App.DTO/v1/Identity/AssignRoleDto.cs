namespace App.DTO.v1.Identity;

public class AssignRoleDto
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }
}