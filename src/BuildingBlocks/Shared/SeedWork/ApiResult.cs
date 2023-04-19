namespace Shared.SeedWork;

public class ApiResult<T>
{
    public bool IsSucceeded { get; set; } = default!;
    public string? Message { get; set; } = default!;
    public T Data { get; }  = default!;
    
    public ApiResult()
    {
    }

    protected ApiResult(bool isSucceeded, string? message = null)
    {
        Message = message;
        IsSucceeded = isSucceeded;
    }

    protected ApiResult(bool isSucceeded, T data, string? message = null)
    {
        Data = data;
        Message = message;
        IsSucceeded = isSucceeded;
    }
}