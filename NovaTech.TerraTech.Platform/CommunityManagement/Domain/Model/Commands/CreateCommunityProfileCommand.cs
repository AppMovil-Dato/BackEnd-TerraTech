using NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.CommunityManagement.Domain.Model.Commands;
public record CreateCommunityProfileCommand(int ProfileId, string Nickname, int ReputationScore, string PublicBio, VisibilityStatus VisibilityStatus);
