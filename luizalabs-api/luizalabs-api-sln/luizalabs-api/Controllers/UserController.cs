using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using LuizaLabsApi.Models;
using LuizaLabsApi.Repositories.Interfaces;
using LuizaLabsApi.Configuration;
using LuizaLabsApi.Dto;
using LuizaLabsApi.Service;

namespace LuizaLabsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IUserVerificationRepository userVerificationRepository;

    public UserController(IUnitOfWork unitOfWork)
    {
        this.userRepository = unitOfWork.User;
        this.userVerificationRepository = unitOfWork.UserVerification;
    }

    [HttpGet]
    public Task<IEnumerable<User>> Get()
    {
        Console.WriteLine(User.Identity.Name);
        return userRepository.All();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(Guid id)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
            return NotFound("User not found");

        user.Password = null;
        return user;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Post([FromBody] CreateUserDto userDto)
    {
        var existingUser = (
            await userRepository.Find(x => x.Email == userDto.Email)
        ).FirstOrDefault();

        if (existingUser != null)
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "Email already in use");

        var user = new User
        {
            Name = userDto.Name,
            Email = userDto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            IsVerified = (int)EnumUserVerification.Unverified,
            Status = (int)EnumUserStatus.Active
        };
        await userRepository.Add(user);

        var userVerification = new UserVerification
        {
            UserId = user.Id,
            Token = Guid.NewGuid(),
            Used = (int)EnumUserVerification.Unverified,
            CreatedAt = DateTime.UtcNow
        };
        await userVerificationRepository.Add(userVerification);

        MailService.SendConfirmationMail(
            user.Email,
            user.Id.ToString(),
            userVerification.Token.ToString()
        );

        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<User>> Put(Guid id, [FromBody] UpdateUserDto userDto)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
            return NotFound("User not found");

        user.Name = userDto.Name ?? user.Name;
        user.Email = userDto.Email ?? user.Email;
        user.IsVerified = userDto.IsVerified ?? user.IsVerified;
        user.Status = userDto.Status ?? user.Status;

        await userRepository.Update(user);

        user.Password = null;
        return user;
    }

    [HttpDelete("{id}")]
    public async Task<bool> Delete(Guid id)
    {
        return await userRepository.Delete(id);
    }

    [HttpPost("{id}/confirm/{token}")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> Confirm(Guid id, Guid token)
    {
        var user = await userRepository.GetById(id);

        if (user == null)
            return NotFound("User not found");

        var userVerification = (
            await userVerificationRepository.Find(
                x =>
                    x.Token == token
                    && x.Used == (int)EnumUsedStatus.NotUsed
                    && x.CreatedAt > DateTime.UtcNow.AddMinutes(-10)
                    && x.UserId == id
            )
        ).FirstOrDefault();

        if (userVerification == null)
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid token or expired");

        user.IsVerified = (int)EnumUserVerification.Verified;

        userVerification.Used = (int)EnumUsedStatus.Used;

        await userRepository.Update(user);
        await userVerificationRepository.Update(userVerification);

        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpPost("password/forgot")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> ForgotPAssaword([FromBody] ForgotPassaword body)
    {
        var user = (await userRepository.Find(x => x.Email == body.Email)).FirstOrDefault();

        if (user == null)
            return NotFound("User not found");

        var userVerification = new UserVerification
        {
            UserId = user.Id,
            Token = Guid.NewGuid(),
            Used = (int)EnumUserVerification.Unverified,
            CreatedAt = DateTime.UtcNow
        };
        await userVerificationRepository.Add(userVerification);

        MailService.PasswordRecovery(
            user.Email,
            user.Id.ToString(),
            userVerification.Token.ToString()
        );

        return Ok("Email sent successfully");
    }

    [HttpPost("password/reset")]
    [AllowAnonymous]
    public async Task<ActionResult<User>> ResetPassword(
        [FromBody] ResetPasswordDto resetPasswordDto
    )
    {
        var user = await userRepository.GetById(resetPasswordDto.UserId);

        if (user == null)
            return NotFound("User not found");

        var userVerification = (
            await userVerificationRepository.Find(
                x =>
                    x.Token == resetPasswordDto.Token
                    && x.Used == (int)EnumUsedStatus.NotUsed
                    && x.CreatedAt > DateTime.UtcNow.AddMinutes(-10)
                    && x.UserId == resetPasswordDto.UserId
            )
        ).FirstOrDefault();

        if (userVerification == null)
            return StatusCode(StatusCodes.Status422UnprocessableEntity, "Invalid token or expired");

        user.Password = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.Password);

        userVerification.Used = (int)EnumUsedStatus.Used;

        await userRepository.Update(user);
        await userVerificationRepository.Update(userVerification);

        return StatusCode(StatusCodes.Status204NoContent);
    }
}
