using System;

namespace JawwedAPI.Core.DTOs;

public class ZekrCategoryResponse
{
    public int CategoryId { get; set; }
    public required string Category { get; set; }
    public required string CategoryAudio { get; set; }
}
