using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.ProfileManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NovaTech.TerraTech.Platform.ProfileManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class ProfileRepository(AppDbContext context) : BaseRepository<Profile>(context), IProfileRepository
{
}
