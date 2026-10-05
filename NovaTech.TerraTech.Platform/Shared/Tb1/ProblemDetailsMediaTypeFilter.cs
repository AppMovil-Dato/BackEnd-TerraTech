using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NovaTech.TerraTech.Platform.Shared.Tb1;
public sealed class ProblemDetailsMediaTypeFilter : IAlwaysRunResultFilter, IOrderedFilter
{
    public int Order => int.MaxValue;

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is not ObjectResult { Value: ProblemDetails } result)
            return;
        result.ContentTypes.Clear();
        result.ContentTypes.Add("application/problem+json");
    }

    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
}
