namespace SPL.WebApp.Domain.DTOs
{
    using System.Collections.Generic;

    public class AllChangingTablesArtifactDTO
    {
        public List<ChangingTablesArtifactDTO> Changetables { get; set; }
        public TapBaanDTO Tapbaan { get; set; }
    }
}
