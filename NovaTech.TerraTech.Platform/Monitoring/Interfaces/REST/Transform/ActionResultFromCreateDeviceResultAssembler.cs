using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaTech.TerraTech.Platform.Monitoring.Application.Errors;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Resources;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;
public static class ActionResultFromCreateDeviceResultAssembler
{
    public static ActionResult ToActionResultFromCreateDeviceResult(Result<Device> result, ControllerBase controller, IStringLocalizer<CommonMessages> localizer, string getDeviceByIdActionName) => result switch
    {
        var success when success.IsSuccess => controller.CreatedAtAction(getDeviceByIdActionName, new { id = success.Value!.Id }, DeviceResourceFromEntityAssembler.ToResourceFromEntity(success.Value!)),
        var failure when failure.IsFailure => failure.Error switch
        {
            CreateDeviceError.DuplicateDevice => controller.Conflict(localizer["DeviceAlreadyExists"].Value),
            CreateDeviceError.FieldNotFound => controller.NotFound(localizer["FieldNotFound"].Value),
            CreateDeviceError.InvalidData => controller.BadRequest(new { error = failure.Message }),
            _ => controller.Problem(title: localizer["UnexpectedServerError"].Value, detail: localizer["UnexpectedErrorCreatingDevice"].Value, statusCode: 500)},
        _ => controller.Problem(title: localizer["UnexpectedServerError"].Value, detail: localizer["UnexpectedErrorProcessingRequest"].Value, statusCode: 500)};
}
