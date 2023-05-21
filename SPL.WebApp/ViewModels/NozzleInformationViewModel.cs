using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SPL.WebApp.ViewModels
{
    public class NozzleInformationViewModel : ValidationAttribute
    {
        // [Required(ErrorMessage = "Requerido")]
        //[DisplayName("No. Serie")]
        [MaxLength(55)]
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("No. Serie")]
        public string NumeroSerie { get; set; }

        [DisplayName("Cant. Total")]
        public int CantidadTotal { get; set; }
    }

   

}
