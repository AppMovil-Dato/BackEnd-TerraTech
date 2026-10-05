using Microsoft.EntityFrameworkCore;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public static class DemoCommands
{
    public static async Task<bool> Run(WebApplication app, AppDbContext db, string[] args)
    {
        var catalog = args.Contains("--demo-catalog");
        var readings = args.Contains("--demo-readings");
        if (!catalog && !readings)
            return false;
        if (!app.Environment.IsDevelopment())
            throw new InvalidOperationException("Demo commands are allowed only in Development.");
        if (catalog)
        {
            for (var i = 1; i <= 5; i++)
            {
                var code = $"TT-ZZZ{i:000}";
                var mac = $"02:54:54:00:00:{i:X2}";
                var existing = await db.Set<SensorCatalogItem>().FindAsync(code);
                if (existing != null)
                {
                    if (existing.MacAddress != mac || !existing.IsDemo)
                        throw new InvalidOperationException($"Catalog collision: {code}");
                    continue;
                }

                if (await db.Set<SensorCatalogItem>().AnyAsync(x => x.MacAddress == mac))
                    throw new InvalidOperationException($"MAC collision: {mac}");
                db.Add(new SensorCatalogItem { SensorCode = code, MacAddress = mac, IsDemo = true });
            }

            await db.SaveChangesAsync();
            Console.WriteLine("Demo catalog ready: TT-ZZZ001 through TT-ZZZ005. No accounts or passwords seeded.");
        }

        if (readings)
        {
            var index = Array.IndexOf(args, "--device-id");
            if (index < 0 || index + 1 >= args.Length || !int.TryParse(args[index + 1], out var id))
                throw new InvalidOperationException("Provide --device-id <registered demo device ID>.");
            var device = await db.Set<Device>().IgnoreQueryFilters().SingleOrDefaultAsync(x => x.Id == id) ?? throw new InvalidOperationException("Register the sensor first.");
            var item = await db.Set<SensorCatalogItem>().SingleAsync(x => x.SensorCode == device.SensorCode);
            if (!item.IsDemo)
                throw new InvalidOperationException("Readings can be generated only for demo sensors.");
            var now = DateTime.UtcNow;
            var end = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, DateTimeKind.Utc);
            var start = end.AddDays(-30).AddHours(1);
            var timestamps = (await db.Set<SensorReading>().IgnoreQueryFilters().Where(x => x.DeviceId == id && x.RecordedAt >= start).Select(x => x.RecordedAt).ToListAsync()).ToHashSet();
            for (var at = start; at <= end; at = at.AddHours(1))
            {
                if (timestamps.Contains(at))
                    continue;
                var phase = (at - DateTime.UnixEpoch).TotalHours;
                db.Add(new SensorReading { DeviceId = id, RecordedAt = at, MoisturePercent = Math.Round(42 + 12 * Math.Sin(phase / 8), 2), SoilTemperatureC = Math.Round(23 + 4 * Math.Sin(phase / 24), 2), NitrogenPpm = 35, PhosphorusPpm = 18, PotassiumPpm = 60, Source = "SIMULATED" });
            }

            await db.SaveChangesAsync();
            Console.WriteLine($"30 days of hourly SIMULATED readings ready for device {id}; existing readings retained.");
        }

        return true;
    }
}
