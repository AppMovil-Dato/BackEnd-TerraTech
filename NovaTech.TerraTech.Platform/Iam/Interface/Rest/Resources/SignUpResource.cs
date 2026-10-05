using System.ComponentModel.DataAnnotations;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;
public record SignUpResource([Required, EmailAddress, MaxLength(255)] string EmailAddress, [Required, MinLength(6), MaxLength(128)] string Password, [Required, MinLength(2), MaxLength(150)] string FullName, [Required] string ConfirmPassword);
