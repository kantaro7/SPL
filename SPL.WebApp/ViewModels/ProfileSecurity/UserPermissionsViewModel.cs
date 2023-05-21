namespace SPL.WebApp.ViewModels.ProfileSecurity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using SPL.WebApp.Domain.DTOs.ProfileSecurity;

    public class UserPermissionsViewModel
    {
        [DisplayName("Perfil")]
        public string Clave { get; set; }
        public List<UserProfilesDTO> UserProfilesDTO { get; set; }
        public List<UserOptionsDTO> UserOptionsDTO { get; set; }
        public List<UserPermissionsDTO> UserPermissionsDTO { get; set; }
        public List<UserTableRow> UserTableRows { get; set; }
        public void LoadUserTableRows()
        {
            if (this.UserPermissionsDTO != null)
            {
                foreach (UserOptionsDTO option in this.UserOptionsDTO)
                {
                    option.checkOption = this.UserPermissionsDTO.Exists(x => x.ClavePerfil == this.Clave && x.ClaveOpcion == Convert.ToString(option.Clave));
                }
            }

            this.UserTableRows = new();
            foreach (UserOptionsDTO father in this.UserOptionsDTO.Where(x => x.ClavePadre is null))
            {
                UserTableRow element = new() { Option = father, SubMenus = new(), Permissions = new(), UserClave = this.Clave };
                if (this.UserOptionsDTO.Exists(x => x.ClavePadre == father.Clave && x.SubMenu == "S"))
                {
                    element.SubMenus = this.UserOptionsDTO.Where(x => x.ClavePadre == father.Clave && x.SubMenu == "S").Select(x => new SubMenusTableRow() { Option = x, Permissions = new() }).OrderBy(x => x.Option.Orden).ToList();
                    
                    foreach (SubMenusTableRow submenu in element.SubMenus)
                    {
                        submenu.Permissions = this.UserOptionsDTO.Where(x => x.ClavePadre == submenu.Option.Clave && x.SubMenu != "S").OrderBy(x => x.Orden).ToList();
                    }
                }
                if (this.UserOptionsDTO.Exists(x => x.ClavePadre == father.Clave && x.SubMenu != "S"))
                {
                    element.Permissions = this.UserOptionsDTO.Where(x => x.ClavePadre == father.Clave && x.SubMenu != "S").OrderBy(x => x.Orden).ToList();
                }
                this.UserTableRows.Add(element);
            }
        }
    }
    public class UserTableRow
    {
        public string UserClave { get; set; }
        public UserOptionsDTO Option { get; set; }
        public List<SubMenusTableRow> SubMenus { get; set; }
        public List<UserOptionsDTO> Permissions { get; set; }
    }
    public class SubMenusTableRow
    {
        public UserOptionsDTO Option { get; set; }
        public List<UserOptionsDTO> Permissions { get; set; }
    }


}

