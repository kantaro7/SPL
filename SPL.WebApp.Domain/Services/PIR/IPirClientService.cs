﻿namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPirClientService
    {
        Task<ApiResponse<List<InfoGeneralTypesReportsDTO>>> GetFilter(string noSerie);

        Task<ApiResponse<ResultISZTestsDTO>> CalculateTestIsz(OutISZTestsDTO dtos);

        Task<ApiResponse<long>> SaveReport(PIRTestsGeneralDTO dto);
    }
}
