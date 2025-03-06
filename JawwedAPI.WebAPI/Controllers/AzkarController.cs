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

    [HttpGet("categories/{id}")]
    public async Task<IActionResult> GetAzkarCategories(int id)
    {
        return Ok(await azkarService.GetZekrById(id));
    }
}
