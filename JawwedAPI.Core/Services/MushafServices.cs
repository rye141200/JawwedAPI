
using JawwedAPI.Core.Domain.Entities;
using JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
namespace JawwedAPI.Core.Services;
public class MushafServices : IMushafServices
{
    public Task<IEnumerable<Line>> GetAllMushafPages()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Line>> GetMushafPage(int pageNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Line>> GetMushafPageVersesPopulated(int pageNumber)
    {
        throw new NotImplementedException();
    }
}
