namespace SPL.WebApp.ViewModels
{
    using Microsoft.AspNetCore.Http;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ETD;

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;

    public class EtdFileLoadViewModel
    {
        public EtdFileLoadViewModel()
        {
        }

        public ETDUploadResultDTO ETDUploadResult { get; set; }

        public List<GraphicETD> Graphics { get; set; }

    }
}
