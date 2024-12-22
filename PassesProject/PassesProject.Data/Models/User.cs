namespace PassesProject.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Avatar { get; set; }
    public string WebsiteUrl { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}