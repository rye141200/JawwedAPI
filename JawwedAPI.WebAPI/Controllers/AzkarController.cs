using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers;

public class AzkarController(IAzkarService azkarService) : CustomBaseController
{
    [HttpGet("categories")]
    public async Task<IActionResult> GetAzkarCategories()
    {
        return Ok(await azkarService.GetAzkarCategories());
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetAzkarFromCategory(int categoryId)
    {
        return Ok(await azkarService.GetZekrById(categoryId));
    }

    [HttpGet("random")]
    public async Task<IActionResult> GetRandomZekr()
    {
        return Ok(await azkarService.GetRandomZekr());
    }

    [HttpGet("{categoryId}/random")]
    public async Task<IActionResult> GetRandomZekrFromCategory(int categoryId)
    {
        return Ok(await azkarService.GetRandomZekrFromCategory(categoryId));
    }
}
