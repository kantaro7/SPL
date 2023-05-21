namespace SPL.WebApp.Domain.Services
{

    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISidcoClientService
    {
        Task<IEnumerable<CatSidcoDTO>> GetCatSIDCO();
    }
}
