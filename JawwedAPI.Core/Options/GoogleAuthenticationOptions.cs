using System;

namespace JawwedAPI.Core.Options;

public class GoogleAuthenticationOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
