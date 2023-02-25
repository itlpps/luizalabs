using System.ComponentModel.DataAnnotations;

namespace LuizaLabsApi.Dto;

public class CreateUserDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(50, ErrorMessage = "Name must be at most 50 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%*()-_=+]).{6,99}",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character"
    )]
    public string? Password { get; set; }
}

public class UpdateUserDto
{
    public string? Name { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; set; }

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%*()-_=+]).{6,99}",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character"
    )]
    public string? Password { get; set; }

    public int? IsVerified { get; set; }

    public int? Status { get; set; }
}

public class ForgotPassaword {
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email")]
    public string Email { get; set; }
}

public class ResetPasswordDto
{
    [Required]
    public Guid UserId { get; set; }

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%*()-_=+]).{6,99}",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number and one special character"
    )]
    public string? Password { get; set; }

    [Required]
    public Guid Token { get; set; }
}
