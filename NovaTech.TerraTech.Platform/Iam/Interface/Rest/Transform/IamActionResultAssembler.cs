using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaTech.TerraTech.Platform.Iam.Domain.Model;
using NovaTech.TerraTech.Platform.Iam.Domain.Model.Aggregates;
using NovaTech.TerraTech.Platform.Shared.Application.Model;
using NovaTech.TerraTech.Platform.Shared.Interfaces.Rest.ProblemDetails;
using NovaTech.TerraTech.Platform.Shared.Resources.Errors;

namespace NovaTech.TerraTech.Platform.Iam.Interface.Rest.Transform;
public static class IamActionResultAssembler
{
    private static int ToStatusCodeFromIamError(IamError error)
    {
        return error switch
        {
            IamError.InvalidCredentials => StatusCodes.Status400BadRequest,
            IamError.UsernameAlreadyTaken => StatusCodes.Status409Conflict,
            IamError.OperationCancelled => StatusCodes.Status409Conflict,
            IamError.DatabaseError => StatusCodes.Status500InternalServerError,
            IamError.InternalServerError => StatusCodes.Status500InternalServerError,
            IamError.ExternalServiceError => StatusCodes.Status503ServiceUnavailable,
            _ => StatusCodes.Status400BadRequest
        };
    }

    public static IActionResult ToActionResultFromSignInResult(ControllerBase controller, Result<(User user, string token)> result, IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory, Func<(User user, string token), IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction(result.Value!);
        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromSignUpResult(ControllerBase controller, Result result, IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory, Func<IActionResult> successAction)
    {
        if (result.IsSuccess)
            return successAction();
        var statusCode = ToStatusCodeFromIamError((IamError)result.Error!);
        return problemDetailsFactory.CreateProblemDetails(controller, statusCode, result.Error, result.Message);
    }

    public static IActionResult ToActionResultFromGetUserByIdResult(ControllerBase controller, User? user, IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory, Func<User, IActionResult> successAction)
    {
        if (user is null)
        {
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromIamError(IamError.UserNotFound), IamError.UserNotFound, errorLocalizer[nameof(IamError.UserNotFound)]);
        }

        return successAction(user);
    }

    public static IActionResult ToActionResultFromGetUserByEmailResult(ControllerBase controller, User? user, IStringLocalizer<ErrorMessages> errorLocalizer, ProblemDetailsFactory problemDetailsFactory, Func<User, IActionResult> successAction)
    {
        if (user is null)
        {
            return problemDetailsFactory.CreateProblemDetails(controller, ToStatusCodeFromIamError(IamError.UserNotFound), IamError.UserNotFound, errorLocalizer[nameof(IamError.UserNotFound)]);
        }

        return successAction(user);
    }
}
