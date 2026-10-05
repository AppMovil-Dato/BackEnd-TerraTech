using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Aggregates;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Application.Services;
public interface IReportQueryService
{
    Task<Report?> GetReportByIdAsync(int reportId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken = default);
}
