namespace SPL.WebApp.Domain.Services.Imp
{
    using SPL.WebApp.Domain.DTOs;
    using System.Collections.Generic;
    using System.Linq;

    public class PlateTensionService : IPlateTensionService
    {
        public bool ValidatePositions(List<PlateTensionDTO> plateTensions, decimal positionAT, decimal positionBT, decimal positionTER, out bool isNewTension)
        {
            bool result = true;
            isNewTension = false;

            decimal countAT = plateTensions.Count(c => c.TipoTension.ToUpper().Equals("AT"));
            decimal countBT = plateTensions.Count(c => c.TipoTension.ToUpper().Equals("BT"));
            decimal countTER = plateTensions.Count(c => c.TipoTension.ToUpper().Equals("TER"));

            if (countAT != 0 || countBT != 0 || countTER != 0)
            {
                countAT = countAT == 0 ? positionAT : countAT;
                countBT = countBT == 0 ? positionBT : countBT;
                countTER = countTER == 0 ? positionTER : countTER;

                if (countAT != positionAT || countBT != positionBT || (countTER != positionTER))
                {
                    isNewTension = true;
                    result = true;
                }
                else
                {
                    isNewTension = false;
                    result = false;
                }
            }
            else
            {
                isNewTension = false;
                result = true;
            }

            return result;
        }
        public bool ValidatePositions(List<ResistDesignDTO> resistDesigns, decimal positionAT, decimal positionBT, decimal positionTER, out bool isNewTension)
        {
            bool result = true;
            isNewTension = false;

            decimal countAT = resistDesigns.Count(c => c.IdSeccion.ToUpper().Equals("AT"));
            decimal countBT = resistDesigns.Count(c => c.IdSeccion.ToUpper().Equals("BT"));
            decimal countTER = resistDesigns.Count(c => c.IdSeccion.ToUpper().Equals("TER"));

            if (countAT != 0 || countBT != 0 || countTER != 0)
            {
                if (countAT != positionAT || countBT != positionBT || countTER != positionTER)
                {
                    isNewTension = true;
                    result = true;
                }
                else
                {
                    isNewTension = false;
                    result = false;
                }
            }
            else
            {
                isNewTension = false;
                result = true;
            }
            return result;
        }
    }
}
