namespace SPL.WebApp.Domain.DTOs
{
    using System;
    using System.Collections.Generic;

    public class RANTestsDetailsDTO
    {
        public List<RANTestsDetailsRADTO> RANTestsDetailsRAs { get; set; }
        public List<RANTestsDetailsTADTO> RANTestsDetailsTAs { get; set; }
   
    }
}
