namespace LuizaLabsApi.Dto;

public class AuthDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class AuthResultDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}

public class TwoFactorAuthDto
{
    public Guid UserId { get; set; }
    public string Token { get; set; }
}
