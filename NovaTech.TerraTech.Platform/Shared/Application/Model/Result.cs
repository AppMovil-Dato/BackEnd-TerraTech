namespace NovaTech.TerraTech.Platform.Shared.Application.Model;

public class Result : Result<object>
{
    // Modified constructor to match the base Result<object> constructor
    private Result(bool isSuccess, string message, Enum? error) : base(isSuccess, null, message, error)
    {
    }

    // Modified Success method to match new constructor
    public static Result Success()
    {
        return new Result(true, string.Empty, null);
    }

    // New Failure method using Enum? and string message
    public new static Result Failure(Enum error, string message)
    {
        return new Result(false, message, error);
    }

    // Removed old Failure(Error error) and Failure(string code, string message) methods.
}