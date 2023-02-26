using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using NUnit.Framework;

using LuizaLabsApi.Configuration;
using LuizaLabsApi.Dto;
using LuizaLabsApi.Models;

namespace luizalabs_tests.Tests;

public class Tests
{
    private ApiApplication application { get; set; }
    private HttpClient client { get; set; }

    private T copy<T>(T obj)
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
    }

    [SetUp]
    public void Setup()
    {
        Params.IsTest = true;
        application = new ApiApplication();
        Database.Create(application).Wait();
        client = application.CreateClient();
    }

    [Test]
    public async Task POST_NewUser_Success()
    {
        var result = await client.PostAsJsonAsync("/api/user", Mock.User);

        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [Test]
    public void POST_NewUser_WeakPassword_Fail()
    {
        var user = copy(Mock.User);
        user.Password = "123456";
        var result = client.PostAsJsonAsync("/api/user", user).Result;

        Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
    }

    [Test]
    public void POST_Login_NotVerified_Fail()
    {
        var user = Mock.User;
        client.PostAsJsonAsync("/api/user", user).Wait();

        var result = client
            .PostAsJsonAsync(
                "api/auth/login",
                new AuthDto { Email = user.Email, Password = user.Password }
            )
            .Result;

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Test]
    public void POST_Login_Fail()
    {
        var user = Mock.User;
        client.PostAsJsonAsync("/api/user", user).Wait();

        var result = client
            .PostAsJsonAsync(
                "api/auth/login",
                new AuthDto { Email = user.Email, Password = "incorrect" }
            )
            .Result;

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Test]
    public void POST_VerifiedUser_Success()
    {
        client.PostAsJsonAsync("/api/user", Mock.User).Wait();

        var user = Database.GetUser(application).Result;
        var userVerification = Database.GetToken(application).Result;

        var result = client
            .PostAsJsonAsync($"api/user/{user.Id}/confirm/{userVerification.Token}", user)
            .Result;

        Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
    }

    [Test]
    public void POST_VerifiedUser_Fail()
    {
        client.PostAsJsonAsync("/api/user", Mock.User).Wait();

        var user = Database.GetUser(application).Result;

        var result = client
            .PostAsJsonAsync($"api/user/{user.Id}/confirm/{Guid.NewGuid()}", user)
            .Result;

        Assert.AreEqual(HttpStatusCode.UnprocessableEntity, result.StatusCode);
    }

    [Test]
    public void POST_Login_Success()
    {
        var user = Mock.User;
        client.PostAsJsonAsync("/api/user", user).Wait();

        Database.VerifyUser(application).Wait();

        var result = client
            .PostAsJsonAsync(
                "api/auth/login",
                new AuthDto { Email = user.Email, Password = user.Password }
            )
            .Result;

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }

    [Test]
    public void POST_2FA_Fail()
    {
        var user = Mock.User;
        client.PostAsJsonAsync("/api/user", user).Wait();

        Database.VerifyUser(application).Wait();

        var loginResponse = client
            .PostAsJsonAsync(
                "api/auth/login",
                new AuthDto { Email = user.Email, Password = user.Password }
            )
            .Result;
        var userLogged = loginResponse.Content.ReadFromJsonAsync<User>().Result;

        var userVerification = Database.GetToken2fa(application).Result;

        var result = client
            .PostAsJsonAsync(
                "api/auth/2fa",
                new TwoFactorAuthDto
                {
                    UserId = userLogged.Id,
                    Token = "incorrect"
                }
            )
            .Result;

        Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Test]
    public void POST_2FA_Success()
    {
        var user = Mock.User;
        client.PostAsJsonAsync("/api/user", user).Wait();

        Database.VerifyUser(application).Wait();

        var loginResponse = client
            .PostAsJsonAsync(
                "api/auth/login",
                new AuthDto { Email = user.Email, Password = user.Password }
            )
            .Result;
        var userLogged = loginResponse.Content.ReadFromJsonAsync<User>().Result;

        var userVerification = Database.GetToken2fa(application).Result;

        var result = client
            .PostAsJsonAsync(
                "api/auth/2fa",
                new TwoFactorAuthDto
                {
                    UserId = userLogged.Id,
                    Token = userVerification.TwoFactorToken
                }
            )
            .Result;

        Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
    }
}
