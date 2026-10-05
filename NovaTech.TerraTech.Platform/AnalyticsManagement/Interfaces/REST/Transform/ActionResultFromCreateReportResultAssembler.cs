using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Application.Errors;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Resources;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Resources;

namespace NovaTech.TerraTech.Platform.AnalyticsManagement.Interfaces.REST.Transform;
public static class ActionResultFromCreateReportResultAssembler
{
    public static ActionResult ToActionResultFromCreateReportResult(Result<Report> result, ControllerBase controller, IStringLocalizer<CommonMessages> localizer, string getReportByIdActionName)
    {
        switch (result)
        {
            case { IsSuccess: true }:
                var resource = ReportResourceFromEntityAssembler.ToResourceFromEntity(result.Value!);
                return controller.CreatedAtAction(getReportByIdActionName, new { id = result.Value!.Id }, resource);
            case { IsFailure: true }:
                return result.Error switch
                {
                    CreateReportError.DuplicateReport => controller.Conflict(localizer["ReportAlreadyExists"].Value),
                    CreateReportError.UnexpectedError => controller.Problem(title: localizer["UnexpectedServerError"].Value, detail: localizer["UnexpectedErrorCreatingReport"].Value, statusCode: 500),
                    _ => controller.Problem(title: localizer["UnexpectedServerError"].Value, detail: localizer["UnexpectedErrorProcessingRequest"].Value, statusCode: 500)};
            default:
                return controller.Problem(title: localizer["UnexpectedServerError"].Value, detail: localizer["UnexpectedErrorProcessingRequest"].Value, statusCode: 500);
        }
    }
}
