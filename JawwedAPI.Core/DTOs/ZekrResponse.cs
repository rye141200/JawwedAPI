using System;

namespace JawwedAPI.Core.DTOs;

public class ZekrResponse
{
    public int CategoryId { get; set; }
    public required string Category { get; set; }
    public required string CategoryAudio { get; set; }

    public required List<ZekrItemResponse> Items { get; set; }
}

public class ZekrItemResponse
{
    public required string Audio { get; set; }
    public required string Content { get; set; }
    public int Count { get; set; }
}
