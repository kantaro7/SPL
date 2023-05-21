using SPL.WebApp.Domain.DTOs.ProfileSecurity;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SPL.WebApp.ViewModels.ProfileSecurity
{
    public partial class AssignmentUsersViewModel
    {

        [DisplayName("Usuario")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        [DisplayName("Nombre")]
        public string NameUser { get; set; }

        [Required(ErrorMessage = "Requerido")]
        [DisplayName("Perfil")]
        public string ClavePerfil { get; set; }

       
        public List<AssignmentUsersDTO> AssignmentUsersDTO { get; set; }
    }
}
