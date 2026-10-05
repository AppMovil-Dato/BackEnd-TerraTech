using NovaTech.TerraTech.Platform.AnalyticsManagement.Application.Services;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Repositories;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Application.Internal.QueryServices;
public class ReportQueryService(IReportRepository reportRepository) : IReportQueryService
{
    public async Task<Report?> GetReportByIdAsync(int reportId, CancellationToken cancellationToken = default) => await reportRepository.FindByIdAsync(reportId, cancellationToken);
    public async Task<IEnumerable<Report>> GetAllReportsAsync(CancellationToken cancellationToken = default) => await reportRepository.ListAsync(cancellationToken);
}
