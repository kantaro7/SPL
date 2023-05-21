namespace SPL.WebApp.Controllers
{
    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Helpers;
    using SPL.WebApp.ViewModels;
    using SPL.WebApp.ViewModels.ProfileSecurity;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileSecurityController : Controller
    {
        private readonly IProfileSecurityService _profileClientService;
     

        private readonly IMapper _mapper;

        private decimal valTension, tensionAT, tensionBT, tensionTER;

        private int? invertidoGrid;
        private DataTable dtValorNom = new();
        private readonly char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        readonly int? idIdentificacion = 0;

        public ProfileSecurityController(
           IProfileSecurityService profileClientService,
            IMapper mapper)
        {
            this._mapper = mapper;
            this._profileClientService = profileClientService;
     
        }

        public IActionResult Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.Seguridad)))
                {

                    return this.View();
                }
                else
                {
                    return View("~/Views/PageConstruction/PermissionDenied.cshtml");

                }

                //var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                //return this.View();
            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                return View("~/Views/PageConstruction/Error.cshtml");


            }
            catch (Exception ex)
            {

                return View("~/Views/PageConstruction/Error.cshtml");

            }

        
        }

     
        [HttpGet]
        public  IActionResult GetProfilesUser(string Profile)
        {
            UserProfilesViewModel UserProfilesViewModel = new();
            ApiResponse<List<UserProfilesDTO>> list =  this._profileClientService.GetProfiles(Profile).Result;
            UserProfilesViewModel.UserProfilesDTO = list.Structure;
            return PartialView("_ProfilesUser", UserProfilesViewModel);
        }

        [HttpPost]
        public IActionResult SaveProfilesUser([FromForm] UserProfilesViewModel viewModel)
        {
            UserProfilesDTO data = new UserProfilesDTO() { Clave = viewModel.Clave, Descripcion = viewModel.Descripcion, Creadopor = " ", Fechacreacion = DateTime.Now };
            ApiResponse<long> list = this._profileClientService.SaveProfiles(data).Result;

            return this.Json(new
            {
                response = list
            });
        }
        [HttpPost]
        public IActionResult DeleteProfilesUser([FromForm] UserProfilesViewModel viewModel)
        {
            UserProfilesDTO data = new UserProfilesDTO() { Clave = viewModel.Clave, Descripcion = viewModel.Descripcion };
            ApiResponse<long> list = this._profileClientService.DeleteProfiles(data).Result;

            return this.Json(new
            {
                response = list
            });
        }









        [HttpGet]
        public IActionResult GetTabPermissionUser()
        {
            UserPermissionsViewModel userPermissionsViewModel = new();
            
            ApiResponse<List<UserOptionsDTO>> listOptionsMenu = this._profileClientService.GetOptionsMenu().Result;


            ApiResponse<List<UserProfilesDTO>> list = this._profileClientService.GetProfiles("-1").Result;


            userPermissionsViewModel.UserOptionsDTO = listOptionsMenu.Structure;

            List<GeneralPropertiesDTO> listProfiles= new List<GeneralPropertiesDTO>();
            foreach (var profile in list.Structure)
            {
                listProfiles.Add(new GeneralPropertiesDTO() { Clave = profile.Clave, Descripcion = profile.Clave });
            }

            ViewBag.Profiles = new SelectList(listProfiles.AsEnumerable(), "Clave", "Descripcion", "");

           
            userPermissionsViewModel.LoadUserTableRows();
            return PartialView("_PermissionsUser", userPermissionsViewModel);
        }

        [HttpGet]
        public JsonResult GetPermissions(string Profile)
        {
            ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionsProfile(Profile).Result;
              


             return this.Json(new
            {
                response = listPermissions.Structure
             });
        }

        [HttpPost]
        public IActionResult SavePermissionUser([FromBody]  List<UserTableRow> viewModel)
        {
            List<UserPermissionsDTO> list = new List<UserPermissionsDTO>();

            if (viewModel.Count() > 0)
            {
                foreach (var item in viewModel)
                {
                    string userclave = viewModel.FirstOrDefault().UserClave;
                    if (item.Option.checkOption)
                    {
                        list.Add(new UserPermissionsDTO() { ClaveOpcion = Convert.ToString(item.Option.Clave), ClavePerfil = userclave });
                    }

                    foreach (var item3 in item.Permissions)
                    {
                        if (item3.checkOption)
                        {
                            list.Add(new UserPermissionsDTO() { ClaveOpcion = Convert.ToString(item3.Clave), ClavePerfil = userclave });
                        }
                    }

                    foreach (var item1 in item.SubMenus)
                    {
                        if (item1.Option.checkOption)
                        {
                            list.Add(new UserPermissionsDTO() { ClaveOpcion = Convert.ToString(item1.Option.Clave), ClavePerfil = userclave });
                        }
                        foreach (var item2 in item1.Permissions)
                        {
                            if (item2.checkOption)
                            {
                                list.Add(new UserPermissionsDTO() { ClaveOpcion = Convert.ToString(item2.Clave), ClavePerfil = userclave });
                            }
                        }
                    }
                }


                if (list.Count() <= 0)
                {
                    list.Add(new UserPermissionsDTO() { ClavePerfil = viewModel.FirstOrDefault().UserClave });
                }



                ApiResponse<long> save = this._profileClientService.SavePermissions(list).Result;

                return this.Json(new
                {
                    response = save
                });
            }
            else
            {

                throw new ArgumentException("No se esta enviando ninguna opción");

            }


           

            
        }

        [HttpGet]
        public IActionResult GetTablePermissionUser(string Profile)
        {
            UserPermissionsViewModel userPermissionsViewModel = new();
            ApiResponse<List<UserOptionsDTO>> listOptionsMenu = this._profileClientService.GetOptionsMenu().Result;
     

            ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionsProfile(Profile).Result;


            userPermissionsViewModel.Clave = Profile;
         
            userPermissionsViewModel.UserOptionsDTO = listOptionsMenu.Structure;
            userPermissionsViewModel.UserPermissionsDTO = listPermissions.Structure;
            userPermissionsViewModel.LoadUserTableRows();

            return PartialView("_TablePermission", userPermissionsViewModel);
        }








        [HttpGet]
        public IActionResult GetTabAsignacionUser()
        {
            AssignmentUsersViewModel assignmentUsersViewModel = new();



            ApiResponse<List<UserProfilesDTO>> list = this._profileClientService.GetProfiles("-1").Result;

            ApiResponse<List<AssignmentUsersDTO>> listAsignacion = this._profileClientService.GetAssignmentProfiles("-1").Result;


            List<GeneralPropertiesDTO> listProfiles = new List<GeneralPropertiesDTO>();
            listProfiles.Add(new GeneralPropertiesDTO() { Clave = "", Descripcion = "Seleccione..." });
            foreach (var profile in list.Structure)
            {
                listProfiles.Add(new GeneralPropertiesDTO() { Clave = profile.Clave, Descripcion = profile.Clave });
            }

            ViewBag.Profiles = new SelectList(listProfiles.AsEnumerable(), "Clave", "Descripcion", "");

            assignmentUsersViewModel.AssignmentUsersDTO = listAsignacion.Structure;
            return PartialView("_AssignmentUser", assignmentUsersViewModel);
        }




        [HttpGet]
        public IActionResult GetTabUser(string name)
        {

        
            ApiResponse<List<UsersDTO>> list = this._profileClientService.GetUsers(name).Result;

          
            return PartialView("_TabUsers", list.Structure);
        }







        [HttpPost]
        public IActionResult SaveAsignacionUser([FromForm] AssignmentUsersViewModel viewModel)
        {
            AssignmentUsersDTO data = new AssignmentUsersDTO() {  ClavePerfil = viewModel.ClavePerfil, Name = viewModel.NameUser, UserId = viewModel.UserId,  Creadopor = " ", Fechacreacion = DateTime.Now };

            ApiResponse<long> list = this._profileClientService.SaveAssignmentProfilesUsers(data).Result;

            return this.Json(new
            {
                response = list
            });
        }
        [HttpPost]
        public IActionResult DeleteAsignacionUser([FromForm] AssignmentUsersViewModel viewModel)
        {
            AssignmentUsersDTO data = new AssignmentUsersDTO() { ClavePerfil = viewModel.ClavePerfil, Name = viewModel.NameUser, UserId = viewModel.UserId, Creadopor = " ", Fechacreacion = DateTime.Now };

            ApiResponse<long> list = this._profileClientService.DeleteAssignmentProfilesUsers(data).Result;

            return this.Json(new
            {
                response = list
            });
        }
    }
}
