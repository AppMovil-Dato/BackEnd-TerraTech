using Microsoft.EntityFrameworkCore.ChangeTracking;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class FieldBoundaryComparer() : ValueComparer<IReadOnlyList<FieldVertex>?>(
    (a, b) => a == null ? b == null : b != null && a.SequenceEqual(b),
    points => points == null ? 0 : points.Aggregate(0, (hash, point) => HashCode.Combine(hash, point.GetHashCode())),
    points => points == null ? null : points.ToArray());
