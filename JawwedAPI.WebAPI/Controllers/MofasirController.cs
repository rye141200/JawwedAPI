using AutoMapper;
using JawwedAPI.Core.DTOs;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JawwedAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MofasirController(IMapper mapper, IMofasirService mofasirService) : ControllerBase
    {
        /// <summary>
        /// Gets mofasir information - returns a specific mofasir if ID is provided, otherwise returns all mofasirs
        /// </summary>
        /// <returns>A single mofasir or list of all mofasirs</returns>
        [HttpGet("info")]
        public async Task<IActionResult> GetMofasirInfo(string? mofasirID = null)
        {
            if (!string.IsNullOrEmpty(mofasirID))
            {
                // Return specific mofasir
                var mofasir = await mofasirService.GetMofasirInfo(mofasirID);
                var response = mapper.Map<MofasirResponse>(mofasir);
                return Ok(response);
            }
            else
            {
                // Return all mofasirs
                var mofasirs = await mofasirService.GetMofasirsInfo();
                var response = mapper.Map<List<MofasirResponse>>(mofasirs);
                return Ok(response);
            }
        }
    }
}
