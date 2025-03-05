using System;
using System.Collections.Generic;

namespace JawwedAPI.Core.DTOs;

/// <summary>
/// Data transfer object that represents a Mofasir (Quranic commentator) for API responses
/// </summary>
public class MofasirResponse
{
    /// <summary>
    /// Unique identifier for the Mofasir
    /// </summary>
    public string MofasirID { get; set; } = string.Empty;

    /// <summary>
    /// The name of the scholar/author
    /// </summary>
    public string AuthorName { get; set; } = string.Empty;

    /// <summary>
    /// The title of the Tafsir book written by this Mofasir
    /// </summary>
    public string BookName { get; set; } = string.Empty;

    /// <summary>
    /// Languages in which this Mofasir's works are available
    /// </summary>
    public string Languages { get; set; } = string.Empty;
}