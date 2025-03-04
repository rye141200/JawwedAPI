using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.DTOs;

public class GoogleLoginRequest
{
    [Required(ErrorMessage = "Token ID is required!")]
    [JwtFormat(ErrorMessage = "JWT must consist of Header, Payload, and Signature")]
    public string IdToken { get; set; } = string.Empty;
}

/// <summary>
/// Validates that a string is in the expected JWT format with three parts separated by periods
/// </summary>
public class JwtFormatAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string token)
            return new ValidationResult("Provided object is not a string");

        // Check if the token has exactly three parts separated by periods
        string[] parts = token.Split('.');
        return parts.Length == 3
            ? ValidationResult.Success
            : new ValidationResult("JWT must consisit of header, payload and signature");
    }
}
