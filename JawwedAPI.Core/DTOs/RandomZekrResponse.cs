using System;

namespace JawwedAPI.Core.DTOs;

public class RandomZekrResponse
{
    public int ZekrID { get; set; }
    public int CategoryId { get; set; }
    public required string Category { get; set; }
    public required string Audio { get; set; }
    public required string Content { get; set; }
    public int Count { get; set; }
}
