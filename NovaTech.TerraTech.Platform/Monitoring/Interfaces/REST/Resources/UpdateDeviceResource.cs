using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Resources;
[SwaggerSchema(Description = "Request payload to update a Device")]
public record UpdateDeviceResource([Required][RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$", ErrorMessage = "MAC address must be in format XX:XX:XX:XX:XX:XX or XX-XX-XX-XX-XX-XX")][SwaggerParameter(Description = "MAC address")] string MacAddress, [Required][SwaggerParameter(Description = "Device status (ONLINE, OFFLINE, LOW_BATTERY)")] string Status, [Required][SwaggerParameter(Description = "Last synchronization timestamp (ISO format)")] DateTimeOffset LastSync);
