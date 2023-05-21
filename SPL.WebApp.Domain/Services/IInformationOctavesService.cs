namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInformationOctavesService
    {
        Task<ApiResponse<List<InformationOctavesDTO>>> GetInfoOctavas(string NroSerie, string TypeInformation, string DateData);

        Task<ApiResponse<long>> SaveOctaves(List<InformationOctavesDTO> data, bool isImport);
    }
}