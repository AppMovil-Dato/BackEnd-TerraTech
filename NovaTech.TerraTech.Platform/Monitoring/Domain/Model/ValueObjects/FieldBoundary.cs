namespace NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

/// <summary>A simple, non-self-intersecting geographic polygon; vertices are not repeated to close it.</summary>
public static class FieldBoundary
{
    public static IReadOnlyList<FieldVertex>? Validate(IReadOnlyList<FieldVertex>? vertices)
    {
        if (vertices is null || vertices.Count == 0) return null;
        var points = vertices.ToList();
        if (points.Count > 1 && points[0] == points[^1]) points.RemoveAt(points.Count - 1);
        if (points.Count is < 3 or > 100 || points.Distinct().Count() != points.Count)
            throw new ArgumentException("A boundary needs 3 to 100 distinct points.");
        if (points.Any(p => !double.IsFinite(p.Latitude) || !double.IsFinite(p.Longitude) || p.Latitude is < -90 or > 90 || p.Longitude is < -180 or > 180))
            throw new ArgumentException("Boundary coordinates are invalid.");
        for (var i = 0; i < points.Count; i++)
        for (var j = i + 1; j < points.Count; j++)
        {
            if (j == i + 1 || i == 0 && j == points.Count - 1) continue;
            if (Intersects(points[i], points[(i + 1) % points.Count], points[j], points[(j + 1) % points.Count]))
                throw new ArgumentException("Boundary edges cannot cross or overlap.");
        }
        if (points.Skip(2).All(p => Math.Abs((points[1].Longitude - points[0].Longitude) * (p.Latitude - points[0].Latitude) - (points[1].Latitude - points[0].Latitude) * (p.Longitude - points[0].Longitude)) < 1e-12))
            throw new ArgumentException("Boundary points cannot be collinear.");
        var area = AreaM2(points);
        if (!double.IsFinite(area) || area < .01 || area > 9999999)
            throw new ArgumentException("Boundary area is outside the supported range.");
        return points.ToArray();
    }

    public static double AreaM2(IReadOnlyList<FieldVertex> points)
    {
        const double radius = 6371009;
        var sum = 0d;
        for (var i = 0; i < points.Count; i++)
        {
            var a = points[i]; var b = points[(i + 1) % points.Count];
            var longitude = (b.Longitude - a.Longitude) * Math.PI / 180;
            if (longitude > Math.PI) longitude -= 2 * Math.PI;
            if (longitude < -Math.PI) longitude += 2 * Math.PI;
            sum += longitude * (2 + Math.Sin(a.Latitude * Math.PI / 180) + Math.Sin(b.Latitude * Math.PI / 180));
        }
        return Math.Abs(sum * radius * radius / 2);
    }

    private static bool Intersects(FieldVertex a, FieldVertex b, FieldVertex c, FieldVertex d)
    {
        static double Cross(FieldVertex p, FieldVertex q, FieldVertex r) =>
            (q.Longitude - p.Longitude) * (r.Latitude - p.Latitude) - (q.Latitude - p.Latitude) * (r.Longitude - p.Longitude);
        static bool On(FieldVertex p, FieldVertex q, FieldVertex r) =>
            Math.Abs(Cross(p, q, r)) < 1e-12 && r.Latitude >= Math.Min(p.Latitude, q.Latitude) && r.Latitude <= Math.Max(p.Latitude, q.Latitude) && r.Longitude >= Math.Min(p.Longitude, q.Longitude) && r.Longitude <= Math.Max(p.Longitude, q.Longitude);
        var ac = Cross(a, b, c); var ad = Cross(a, b, d); var ca = Cross(c, d, a); var cb = Cross(c, d, b);
        return ac * ad < 0 && ca * cb < 0 || On(a, b, c) || On(a, b, d) || On(c, d, a) || On(c, d, b);
    }
}
