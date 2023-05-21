namespace SPL.WebApp.Domain.Services
{
    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITestClientService
    {
        Task<ApiResponse<IEnumerable<TestsDTO>>> GetTest(string typeReport, string keyTest);
    }
}
