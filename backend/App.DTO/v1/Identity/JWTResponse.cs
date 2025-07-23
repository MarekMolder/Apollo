namespace App.DTO.v1.Identity;

public class JWTResponse
{
    public Guid UserId { get; set; }
    public string JWT { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public string Email { get; set; } = default!;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
    
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
}