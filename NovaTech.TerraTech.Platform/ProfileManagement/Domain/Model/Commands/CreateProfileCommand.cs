namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
public record CreateProfileCommand(int UserId, string FundoName, string ContactPhone, double MoistureThreshold, double TempThreshold);
