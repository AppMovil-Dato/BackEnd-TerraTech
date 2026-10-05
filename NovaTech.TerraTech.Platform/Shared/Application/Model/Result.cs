namespace NovaTech.TerraTech.Platform.Shared.Application.Model;
public class Result : Result<object>
{
    private Result(bool isSuccess, string message, Enum? error) : base(isSuccess, null, message, error)
    {
    }

    public static Result Success()
    {
        return new Result(true, string.Empty, null);
    }

    public new static Result Failure(Enum error, string message)
    {
        return new Result(false, message, error);
    }
}
