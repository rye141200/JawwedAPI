using System;

namespace JawwedAPI.Core.Exceptions.CustomExceptions;

public interface IErrorDetails
{
    int StatusCode { get; }
    public string? Title { get; set; }
    public string? Detail { get; set; }
}
