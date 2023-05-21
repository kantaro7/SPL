namespace SPL.WebApp.Domain.Services
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;

    public interface IPlateTensionService
    {
        bool ValidatePositions(List<PlateTensionDTO> plateTensions, decimal positionAT, decimal positionBT, decimal positionTER, out bool isNewTension);
        bool ValidatePositions(List<ResistDesignDTO> plateTensions, decimal positionAT, decimal positionBT, decimal positionTER, out bool isNewTension);
    }
}
