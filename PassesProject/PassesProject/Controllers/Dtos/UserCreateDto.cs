namespace PassesProject.Controllers.Dtos;

public class UserCreateDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public string WebsiteUrl { get; set; }
}