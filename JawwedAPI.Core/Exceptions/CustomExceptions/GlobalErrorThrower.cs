namespace JawwedAPI.Core.Exceptions.CustomExceptions;

public class GlobalErrorThrower : Exception
{
    public int StatusCode { get; set; }
    public string Title { get; set; }
    public string Detail { get; set; }

    public GlobalErrorThrower(
        int statusCode = 500,
        string title = "SomeThing Went wrong !",
        string detail = "Hold it we are trying to fix this problem"
    )
        : base(detail)
    {
        StatusCode = statusCode;
        Title = title;
        Detail = detail;
    }
}
