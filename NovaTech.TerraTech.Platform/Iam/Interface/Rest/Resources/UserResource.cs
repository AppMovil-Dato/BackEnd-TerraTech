namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Resources;
public record UserResource(int Id, string EmailAddress, string? FullName = null);
