using System;
using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.DTOs;

public class GoogleLoginRequest
{
    [Required(ErrorMessage = "Token ID is required!")]
    public string IdToken { get; set; } = string.Empty;
}
