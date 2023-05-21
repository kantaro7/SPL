namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INozzleBrandTypeClientService
    {
        Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleTypesByBrand(Int64 pIdMark);
        Task<ApiResponse<long>> SaveRegisterNozzleTypeMark(NozzleMarksDTO dto);
        Task<ApiResponse<long>> DeleteTypeNozzleMarks(NozzleMarksDTO dto);
        Task<ApiResponse<List<NozzleMarksDTO>>> GetBrandType(TypeNozzleMarksDTO request);
    }
}
