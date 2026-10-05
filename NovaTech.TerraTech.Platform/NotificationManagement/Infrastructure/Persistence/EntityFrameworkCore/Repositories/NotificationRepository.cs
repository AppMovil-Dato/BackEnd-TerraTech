using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.NotificationManagement.Domain.Repositories;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace NovaTech.TerraTech.Platform.NotificationManagement.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    private readonly AppDbContext _context;
    public NotificationRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Notification>> FindByProfileIdAsync(int profileId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Notification>().Where(n => n.ProfileId == profileId).OrderByDescending(n => n.CreatedAt).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Notification>> FindUnreadByProfileIdAsync(int profileId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Notification>().Where(n => n.ProfileId == profileId && !n.IsRead).OrderByDescending(n => n.CreatedAt).ToListAsync(cancellationToken);
    }
}
