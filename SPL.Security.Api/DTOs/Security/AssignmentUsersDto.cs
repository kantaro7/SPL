﻿using System;
using System.Collections.Generic;

#nullable disable

namespace SPL.Security.Api.DTOs.Security
{
    public partial class AssignmentUsersDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ClavePerfil { get; set; }
        public string Creadopor { get; set; }
        public DateTime Fechacreacion { get; set; }
        public string Modificadopor { get; set; }
        public DateTime? Fechamodificacion { get; set; }
    }
}
