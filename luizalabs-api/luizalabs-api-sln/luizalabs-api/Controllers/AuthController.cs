using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using LuizaLabsApi.Configuration;
using LuizaLabsApi.Dto;
using LuizaLabsApi.Repositories.Interfaces;
using LuizaLabsApi.Service;
using LuizaLabsApi.Models;

namespace LuizaLabsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IUserVerificationRepository userVerificationRepository;

    public AuthController(IUnitOfWork unitOfWork)
    {
        this.userRepository = unitOfWork.User;
        this.userVerificationRepository = unitOfWork.UserVerification;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("login")]
    public async Task<ActionResult<User>> Post([FromBody] AuthDto body)
    {
        var user = (
            await userRepository.Find(
                x => x.Email == body.Email && x.IsVerified == (int)EnumUserVerification.Verified
            )
        ).FirstOrDefault();

        if (user == null)
            return NotFound("User or password not found");

        if (!BCrypt.Net.BCrypt.Verify(body.Password, user.Password))
            return NotFound("User or password not found");

        var token2FA = Guid.NewGuid().ToString("n").Substring(0, 6).ToUpper();

        var userVerification = new UserVerification
        {
            UserId = user.Id,
            TwoFactorToken = token2FA,
            Used = (int)EnumUsedStatus.NotUsed,
            CreatedAt = DateTime.UtcNow
        };
        await userVerificationRepository.Add(userVerification);

        MailService.TwoFactorAuth(user.Email, token2FA);

        user.Password = null;
        return user;
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("2fa")]
    public async Task<ActionResult<AuthResultDto>> Post([FromBody] TwoFactorAuthDto body)
    {
        var user = (
            await userRepository.Find(
                x => x.Id == body.UserId && x.IsVerified == (int)EnumUserVerification.Verified
            )
        ).FirstOrDefault();

        if (user == null)
            return NotFound("User not found");

        var userVerification = (
            await userVerificationRepository.Find(
                x =>
                    x.UserId == user.Id
                    && x.Used == (int)EnumUsedStatus.NotUsed
                    && x.CreatedAt > DateTime.UtcNow.AddMinutes(-5)
                    && x.TwoFactorToken == body.Token.ToUpper()
            )
        ).FirstOrDefault();

        if (userVerification == null)
            return NotFound("Invalid token");

        userVerification.Used = (int)EnumUsedStatus.Used;

        await userVerificationRepository.Update(userVerification);

        var token = AuthService.GenerateToken(user);

        return new AuthResultDto
        {
            Email = user.Email,
            Name = user.Name,
            Token = token
        };
    }
}
