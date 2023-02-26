using LuizaLabsApi.Models;

public static class Mock
{
    public static Guid UserId = Guid.NewGuid();
    public static User User = new User
    {
        Name = "Teste",
        Email = "mail@mail.com",
        Password = "abc123ABC!@#",
    };
}
