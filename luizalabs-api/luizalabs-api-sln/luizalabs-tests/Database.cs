using LuizaLabsApi.Models;
using Microsoft.Extensions.DependencyInjection;

namespace luizalabs_tests.Tests;

public class Database
{
    public static async Task Create(ApiApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<DatabaseContext>())
            {
                await DbContext.Database.EnsureCreatedAsync();
            }
        }
    }

    public static async Task<UserVerification> GetToken(ApiApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<DatabaseContext>())
            {
                return DbContext.UserVerification.FirstOrDefault(x => x.Token != null);
            }
        }
    }

    public static async Task<UserVerification> GetToken2fa(ApiApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<DatabaseContext>())
            {
                return DbContext.UserVerification.FirstOrDefault(x => x.TwoFactorToken != null);
            }
        }
    }

    public static async Task<User> GetUser(ApiApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<DatabaseContext>())
            {
                return DbContext.User.FirstOrDefault();
            }
        }
    }

    public static async Task VerifyUser(ApiApplication application)
    {
        using (var scope = application.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<DatabaseContext>())
            {
                var user = DbContext.User.FirstOrDefault();
                user.IsVerified = 1;
                DbContext.User.Update(user);
                DbContext.SaveChanges();
            }
        }
    }
}
