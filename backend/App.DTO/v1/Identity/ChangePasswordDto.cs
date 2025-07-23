namespace App.DTO.v1.Identity;

public class ChangePasswordDto
{
    public string Email { get; set; } = default!;
    public string CurrentPassword { get; set; } = default!;
    public string NewPassword { get; set; } = default!;
}