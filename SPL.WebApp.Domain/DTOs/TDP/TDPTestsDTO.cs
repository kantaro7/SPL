namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class TDPTestsDTO
    {
        public List<TDPTestsDetailsDTO> TDPTestsDetails { get; set; }

        public TDPCalibrationMeasurementDTO TDPTestsDetailsCalibration { get; set; }
    }
}
