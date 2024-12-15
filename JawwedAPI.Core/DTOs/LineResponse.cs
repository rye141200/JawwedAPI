using System;
using JawwedAPI.Core.Domain.Entities;

namespace JawwedAPI.Core.DTOs;

public class LineResponse
{
    public int LineID { get; set; }

    public int LineNumber { get; set; }

    public string? LineType { get; set; }

    public int SurahNumber { get; set; }

    public int PageNumber { get; set; }


    public string? Text { get; set; }

    public bool IsCentered { get; set; }

    public string? VersesKeys { get; set; }

    public int JuzNumber { get; set; }
    public int HizbNumber { get; set; }
    public int RubHizbNumber { get; set; }
}
