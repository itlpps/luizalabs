using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuizaLabsApi.Models;

public enum EnumUsedStatus
{
    NotUsed = 0,
    Used = 1
}

public class UserVerification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public Guid? Token { get; set; }

    public string? TwoFactorToken { get; set; }

    public Guid UserId { get; set; }

    [EnumDataType(typeof(EnumUsedStatus))]
    public int Used { get; set; }

    public DateTime CreatedAt { get; set; }
}