using System;

namespace JawwedAPI.Core.Options;

public class AudioAssetsOptions
{
    public string LinkExampleMurattal { get; set; } = string.Empty;
    public string LinkExampleSimple { get; set; } = string.Empty;
    public string AudioBaseLink { get; set; } = string.Empty;
    public List<string> Mashaykh { get; set; } = new List<string>();
}
