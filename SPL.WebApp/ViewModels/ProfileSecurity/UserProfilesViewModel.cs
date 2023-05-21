using SPL.WebApp.Domain.DTOs.ProfileSecurity;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPL.WebApp.ViewModels.ProfileSecurity
{
    public class UserProfilesViewModel
    {
        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Perfil")]
        [MaxLength(30, ErrorMessage = "El perfil no puede excederse de 30 caracteres")]
        [MinLength(1)]
        [RegularExpression(@"([a-zA-Z0-9_\s]+)", ErrorMessage = "El perfil debe ser alfanumerica")]
        public string Clave { get; set; }
        [Required(ErrorMessage = "Requerido")]

        [DisplayName("Descripción")]
        [MaxLength(80, ErrorMessage = "La descripción no puede excederse de 80 caracteres")]
        [MinLength(1)]
        [RegularExpression(@"([a-zA-Z0-9-_\s]+)", ErrorMessage = "La descripción debe ser alfanumerica")]
        public string Descripcion { get; set; }
        public List<UserProfilesDTO> UserProfilesDTO { get; set; }


    }
}
