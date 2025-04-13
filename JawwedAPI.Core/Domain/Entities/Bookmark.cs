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

    /// <summary>
    ///the verse key associated with the bookmark.
    /// Ex: 1:2 , 2:4 verse:chapter
    /// </summary>
    public string? VerseKey { get; set; }
    public string? Verse { get; set; }
    public string? Page { get; set; }

    //! navigational property
    [Required]
    public Guid UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public ApplicationUser? ApplicationUser { get; set; }
}
