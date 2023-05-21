namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IResistanceTwentyDegreesClientServices
    {
        Task<ApiResponse<List<ResistDesignDTO>>> GetResistDesignDTO(
            string noSerie,
            string unitMeasurement,
            string TestConnection,
            decimal temperature,
            string idSection = "-1",
            decimal order = -1);

        Task<ApiResponse<List<ResistDesignDTO>>> GetResistDesignCustom(
           string noSerie,
           string unitMeasurement,
           string TestConnection,
           decimal temperature,
           string idSection,
           decimal order = -1);
    }
}
