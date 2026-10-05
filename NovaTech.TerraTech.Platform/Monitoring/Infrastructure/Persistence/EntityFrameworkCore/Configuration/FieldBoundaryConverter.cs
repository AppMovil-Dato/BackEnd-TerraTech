using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.ValueObjects;

namespace NovaTech.TerraTech.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
public class FieldBoundaryConverter() : ValueConverter<IReadOnlyList<FieldVertex>?, string?>(points => JsonSerializer.Serialize(points, (JsonSerializerOptions? )null), json => JsonSerializer.Deserialize<FieldVertex[]>(json!, (JsonSerializerOptions? )null)!);
