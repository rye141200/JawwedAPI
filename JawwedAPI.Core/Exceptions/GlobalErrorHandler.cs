using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace JawwedAPI.Core.Exceptions;

public class GlobalErrorHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken
    )
    {
        HttpResponse response = httpContext.Response;
        Type exceptionType = exception.GetType();

        //! 1) extract data from the excption
        int statusCode = (int)(
            exceptionType?.GetProperty("StatusCode")?.GetValue(exception) ?? 500
        );
        string title = (string)(
            exceptionType?.GetProperty("Title")?.GetValue(exception) ?? "Unkown error"
        );
        string detail = (string)(
            exceptionType?.GetProperty("Detail")?.GetValue(exception)
            ?? "Something went wrong hold it we are fixing it !"
        );
        //! 2) construct the error message
        response.ContentType = "application/json";
        response.StatusCode = statusCode;
        string result = JsonSerializer.Serialize(
            new
            {
                StatusCode = statusCode,
                Title = title,
                Detail = detail,
            }
        );
        //! 3) write the error to the user
        await response.WriteAsync(result);
        return true;
    }
}
