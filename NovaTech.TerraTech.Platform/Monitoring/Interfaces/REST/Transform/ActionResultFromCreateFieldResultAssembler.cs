using NovaTech.TerraTech.Platform.Shared.Resources;
using NovaTech.TerraTech.Platform.Monitoring.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Monitoring.Application.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaTech.TerraTech.Platform.Shared.Application.Model;

namespace NovaTech.TerraTech.Platform.Monitoring.Interfaces.REST.Transform;

public class ActionResultFromCreateFieldResultAssembler
{
    public static ActionResult ToActionResultFromCreateFieldResult(
        Result<Field> result,
        ControllerBase controller,
        IStringLocalizer<CommonMessages> localizer,
        string getFieldByIdActionName) =>
        result switch
        {
            var success when success.IsSuccess =>
                controller.CreatedAtAction(getFieldByIdActionName, new { id = success.Value!.Id },
                    FieldResourceFromEntityAssembler.ToResourceFromEntity(success.Value!)),

            var failure when failure.IsFailure =>
                failure.Error switch
                {
                    CreateFieldError.InvalidData =>
                        NovaTech.TerraTech.Platform.Shared.Tb1.ApiFailure.Result(400, "INVALID_INPUT", "Field data or boundary is invalid."),

                    CreateFieldError.DuplicateField =>
                        controller.Conflict(localizer["NewsFieldDuplicated"].Value),

                    CreateFieldError.UnexpectedError =>
                        controller.Problem(
                            title: localizer["UnexpectedServerError"].Value,
                            detail: localizer["UnexpectedErrorCreatingField"].Value,
                            statusCode: 500),

                    _ => controller.Problem(
                        title: localizer["UnexpectedServerError"].Value,
                        detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                        statusCode: 500)
                },

            _ => controller.Problem(
                title: localizer["UnexpectedServerError"].Value,
                detail: localizer["UnexpectedErrorProcessingRequest"].Value,
                statusCode: 500)
        };
    
}