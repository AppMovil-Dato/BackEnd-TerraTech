using Microsoft.AspNetCore.Mvc;
namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public class ApiFailure(int status, string code, string message) : Exception(message)
{
    public int Status { get; } = status;
    public string Code { get; } = code;
    public static ObjectResult Result(int status, string code, string message) => new(new ProblemDetails { Status = status, Title = message, Extensions = { ["code"] = code } }) { StatusCode = status };
}
