using System;
using JawwedAPI.Core.Exceptions.CustomExceptions;

namespace JawwedAPI.Core.Exceptions;

public class DataValidationException : Exception, IErrorDetails
{
    public int StatusCode { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }

    public DataValidationException(int StatusCode, string? Title, string? Detail) : base(Detail)
    {
        this.StatusCode = StatusCode;
        this.Title = Title;
        this.Detail = Detail;
    }
}