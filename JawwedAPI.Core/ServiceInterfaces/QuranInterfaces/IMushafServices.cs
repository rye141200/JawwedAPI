
using JawwedAPI.Core.Domain.Entities;

namespace JawwedAPI.Core.ServiceInterfaces.QuranInterfaces;
public interface IMushafServices
{
    Task<IEnumerable<Line>> GetAllMushafPages();
    Task<IEnumerable<Line>> GetMushafPage(int pageNumber);
    Task<IEnumerable<Line>> GetMushafPageVersesPopulated(int pageNumber);
}
