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
        [HttpGet("mofasir")]
        public async Task<ActionResult<MofasirResponse>> getMofasirInfo(string mofasirID)
        {
            MofasirResponse mofasirResponse = mapper.Map<MofasirResponse>(await mofasirService.GetMofasirInfo(mofasirID));
            return mofasirResponse;
        }
        [HttpGet("mofasirs")]
        public async Task<ActionResult<List<MofasirResponse>>> getMofasirsInfo()
        {
            List<MofasirResponse> mofasirsResponse = mapper.Map<List<MofasirResponse>>(await mofasirService.GetMofasirsInfo());
            return mofasirsResponse;
        }
    }
}
