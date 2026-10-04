using System.ComponentModel.DataAnnotations;
namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;
public record SignInResource([Required, EmailAddress] string EmailAddress, [Required] string Password);
