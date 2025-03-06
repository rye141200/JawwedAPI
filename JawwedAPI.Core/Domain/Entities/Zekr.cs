using System.ComponentModel.DataAnnotations;

namespace JawwedAPI.Core.Domain.Entities;

public class Zekr
{
    [Key]
    [Required]
    public int ZekrID { get; set; }

    [Required]
    public required string Category { get; set; }

    [Required]
    public required string CategoryAudio { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required string Audio { get; set; }

    [Required]
    public int Count { get; set; }

    [Required]
    public int CategoryId { get; set; }
}
