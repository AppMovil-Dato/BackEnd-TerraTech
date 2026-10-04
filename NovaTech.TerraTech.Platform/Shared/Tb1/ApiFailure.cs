using Microsoft.AspNetCore.Mvc;
namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public class ApiFailure(int status, string code, string message) : Exception(message)
{
    public int Status { get; } = status;
    public string Code { get; } = code;
    public static ObjectResult Result(int status, string code, string message) => new(new ProblemDetails { Status = status, Title = message, Extensions = { ["code"] = code } }) { StatusCode = status };
}
public class ApiExceptionHandler : Microsoft.AspNetCore.Diagnostics.IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        var status = exception is ApiFailure f ? f.Status : exception is ArgumentException ? 400 : exception is Microsoft.EntityFrameworkCore.DbUpdateException { InnerException: MySql.Data.MySqlClient.MySqlException { Number: 1062 or 1451 or 1452 } } ? 409 : 500;
        var code = exception is ApiFailure a ? a.Code : status == 409 ? "DATA_CONFLICT" : status == 400 ? "INVALID_INPUT" : "INTERNAL_ERROR";
        context.Response.StatusCode = status;
        await Results.Problem(statusCode: status, title: status == 500 ? "An internal error occurred." : exception is ApiFailure ? exception.Message : "The request could not be completed.", extensions: new Dictionary<string, object?> { ["code"] = code }).ExecuteAsync(context);
        return true;
    }
}
