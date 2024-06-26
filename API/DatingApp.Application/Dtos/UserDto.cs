namespace DatingApp.Application.Dtos;

public class UserDto
{
    public string Username { get; set; }
    public string Token { get; set; }
    public string PhotoUrl { get; set; }
    public string KnownAs { get; set; }
    public string Gender { get; set; }
    public bool EmailConfirmed { get; set; }
}