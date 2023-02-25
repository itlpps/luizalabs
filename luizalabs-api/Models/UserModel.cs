using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuizaLabsApi.Models;

public enum EnumUserStatus
{
    [Display(Name = "Ativo")]
    Active = 1,
    [Display(Name = "Inativo")]
    Inactive = 0
}

public enum EnumUserVerification
{
    [Display(Name = "Verificado")]
    Verified = 1,
    [Display(Name = "Não verificado")]
    Unverified = 0
}


public class User
{
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }
    
    [Required]
    [MaxLength(255)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [EnumDataType(typeof(EnumUserVerification))]
    public int? IsVerified { get; set; }

    [Required]
    [EnumDataType(typeof(EnumUserStatus))]
    public int? Status { get; set; }
}
