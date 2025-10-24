namespace SmartInventory.Api.Models;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
}
