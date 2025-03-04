using System;

namespace JawwedAPI.Core.Options;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public long ExpirationMinutes { get; set; }
}
