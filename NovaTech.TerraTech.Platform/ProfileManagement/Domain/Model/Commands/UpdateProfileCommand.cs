namespace NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Commands;
public record UpdateProfileCommand(int Id, string FundoName, string ContactPhone, double MoistureThreshold, double TempThreshold);
