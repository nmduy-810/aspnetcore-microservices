namespace Shared.SeedWork;

public class ApiErrorResult<T> : ApiResult<T>
{
    public List<string> Errors { set; get; } = default!;

    public ApiErrorResult() : this("Something wrong happened. Please try again later")
    {
    }

    public ApiErrorResult(string message) : base(false, message)
    {
    }

    public ApiErrorResult(List<string> errors) : base(false)
    {
        Errors = errors;
    }
}