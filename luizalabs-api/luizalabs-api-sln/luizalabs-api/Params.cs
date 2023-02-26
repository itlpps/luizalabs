namespace LuizaLabsApi.Configuration;

public static class Params {
    public static string SecretKeyJWT { get; set; }
    public static string Email { get; set; }
    public static string EmailPassword { get; set; }

    public static bool IsTest = false; 
}
