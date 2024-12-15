using System;
using JawwedAPI.Core.Exceptions.CustomExceptions;

namespace JawwedAPI.Core.Exceptions;

public class InternalServerErrorException : Exception, IErrorDetails
{
    public int StatusCode { get; set; }
    public string? Title { get; set; }
    public string? Detail { get; set; }

    public InternalServerErrorException(int StatusCode, string? Title, string? Detail) : base(Detail)
    {
        this.StatusCode = StatusCode;
        this.Title = Title;
        this.Detail = Detail;
    }

}