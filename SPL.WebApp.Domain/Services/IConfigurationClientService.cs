using SPL.Domain;
using SPL.WebApp.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.Services
{
    public interface IConfigurationClientService
    {
        Task<ApiResponse<List<InformationLaboratoriesDTO>>> GetInformationLaboratories();
    }
}
