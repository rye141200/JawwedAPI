using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace JawwedAPI.Core.Domain.Entities;
/// <summary>
/// Represents a bookmark entity.
/// </summary>
[Table("Bookmarks")]
public class Bookmark
{
    [Key]
    public int BookmarkId { get; set; }
    [Required]
    public int UserId { get; set; }
    /// <summary>
    ///the verse key associated with the bookmark.
    /// Ex: 1:2 , 2:4 verse:chapter
    /// </summary>
    public string? VerseKey { get; set; }
    public string? Verse { get; set; }
    public string? Page { get; set; }

}
