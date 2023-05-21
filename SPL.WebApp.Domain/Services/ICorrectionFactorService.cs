namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;

    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICorrectionFactorService
    {
     
        Task<ApiResponse<List<CorrectionFactorDTO>>> GetAllDataFactor(string Specification , decimal Temperature ,decimal CorrectionFactor);
        Task<ApiResponse<long>> SaveCorrectionFactorSpecification(CorrectionFactorDTO request);
        Task<ApiResponse<long>> SaveCorrectionFactorsXMarksXTypes(CorrectionFactorsXMarksXTypesDTO request);
        Task<ApiResponse<long>> DeleteCorrectionFactorSpecification(CorrectionFactorDTO request);
        Task<ApiResponse<long>> DeleteRegisterNozzleTypeMark(CorrectionFactorsXMarksXTypesDTO request);
        Task<ApiResponse<List<NozzleMarksDTO>>> GetNozzleMarks(NozzleMarksDTO request);
        Task<ApiResponse<List<TypeNozzleMarksDTO>>> GetTypeXMarksNozzle(TypeNozzleMarksDTO request);
        Task<ApiResponse<List<CorrectionFactorsXMarksXTypesDTO>>> GetCorrectionFactorsXMarksXTypes();
        Task<ApiResponse<List<ValidationTestsIszDTO>>> GetValidationTestsISZ();
 
    }
}
