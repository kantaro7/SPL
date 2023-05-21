using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;

using SPL.Domain;
using SPL.Domain.SPL.Configuration;
using SPL.WebApp.Domain.DTOs;
using SPL.WebApp.Domain.DTOs.ProfileSecurity;
using SPL.WebApp.Domain.Enums;
using SPL.WebApp.Domain.Services;
using SPL.WebApp.Domain.Services.ProfileSecurity;
using SPL.WebApp.Helpers;
using SPL.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SPL.WebApp.Controllers
{
    public class NozzleInformationController : Controller
    {

        private readonly IMapper _mapper;
        private INozzleInformationService _nozzleInformationService;
        private ICorrectionFactorService _configurationInfrastructure;
        private readonly IProfileSecurityService _profileClientService;
        public NozzleInformationController(IMapper mapper , ICorrectionFactorService configurationInfrastructure, INozzleInformationService nozzleInformationService,
            IProfileSecurityService profileClientService)
        {
            this._mapper = mapper;
            this._nozzleInformationService = nozzleInformationService;
            this._configurationInfrastructure = configurationInfrastructure;
            this._profileClientService = profileClientService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(User.Identity.Name).Result;


                if (listPermissions.Structure.Exists(x => Convert.ToInt32(x.ClaveOpcion) == Convert.ToInt32(SPL.WebApp.Domain.Enums.Menu.InformacióndeBoquillas)))
                {

                    var result = await this._configurationInfrastructure.GetNozzleMarks(new NozzleMarksDTO { IdMarca = -1, Estatus = true });
                    //var firstRecord = new List<NozzleMarksDTO>() { new NozzleMarksDTO { Descripcion = "Seleccionar", IdMarca = 0 } };
                    //firstRecord.AddRange(result.Structure);
                    ViewBag.NozzleMarks = result.Structure;

                    var response = await this._configurationInfrastructure.GetCorrectionFactorsXMarksXTypes();
                    response.Structure = response.Structure.OrderByDescending(x => x.Fechacreacion).ToList();
                    var marcas = await this._configurationInfrastructure.GetNozzleMarks(new NozzleMarksDTO { IdMarca = -1, Estatus = true });

                    List<TypeNozzleMarksDTO> tipo = new List<TypeNozzleMarksDTO>() {
                new TypeNozzleMarksDTO { IdTipo =0 , Descripcion ="Seleccione..." }
            };

                    List<NozzleMarksDTO> marca = new List<NozzleMarksDTO>()
            {
                 new NozzleMarksDTO { IdMarca =0 , Descripcion ="Seleccione..." }
            };

                    marca.AddRange(marcas.Structure);

                    ViewBag.Marca = new SelectList(marca.AsEnumerable(), "IdMarca", "Descripcion", "");
                    ViewBag.Tipo = new SelectList(tipo.AsEnumerable(), "IdTipo", "Descripcion", "");

                    var noSerie = ParserHelper.GetNoSerieFromUrlQuery(Request.Query);
                    ViewBag.NoSerie = noSerie;

                    return await Task.Run(() => View());
                }
                else
                {
                    return View("~/Views/PageConstruction/PermissionDenied.cshtml");

                }
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
        public async Task<ApiResponse<NozzlesByDesignDTO>> GetRecordNozzleInformation(string numeroSerie)
        {
           
                var result = await this._nozzleInformationService.GetRecordNozzleInformation(numeroSerie);
            result.Structure.NozzleInformation = result.Structure.NozzleInformation.OrderBy(x=>x.Orden).ToList();

            for(int i = 0; i < result.Structure.NozzleInformation.Count; i++)
            {
                if (result.Structure.NozzleInformation[i].FactorPotencia2 == 0)
                {
                    result.Structure.NozzleInformation[i].FactorPotencia2 = null;
                }

                if (result.Structure.NozzleInformation[i].Capacitancia2 == 0)
                {
                    result.Structure.NozzleInformation[i].Capacitancia2 = null;
                }

                
            }
                return result;

            

        }

        [HttpGet]
        public async Task<List<TypeNozzleMarksDTO>> GetTypeXMarksNozzle(long IdMarca)
        {
            var firstRecord = new List<TypeNozzleMarksDTO>() { new TypeNozzleMarksDTO { Descripcion = "Seleccionar", IdTipo = 0 } };
            var result = await this._configurationInfrastructure.GetTypeXMarksNozzle(new TypeNozzleMarksDTO {  Estatus =true, IdMarca = IdMarca });
            firstRecord.AddRange(result.Structure);

            return firstRecord;
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecordNozzleInformation([FromBody] NozzlesByDesignDTO viewModel)
        {
            try
            {
                for (int i = 0; i < viewModel.NozzleInformation.Count; i++)
                {
                    if (viewModel.NozzleInformation[i].FactorPotencia2 == null)
                    {
                        viewModel.NozzleInformation[i].FactorPotencia2 = 0;
                    }

                    if (viewModel.NozzleInformation[i].Capacitancia2 == null)
                    {
                        viewModel.NozzleInformation[i].Capacitancia2 = 0;
                    }


                }

                if (viewModel.OperationType)
                {
                    viewModel.NozzleInformation.ForEach(x =>
                    {
                        x.Creadopor = User.Identity.Name;
                        x.Fechacreacion = DateTime.Now;
                        x.Modificadopor = null;
                        x.Fechamodificacion = null;
                    });
                }
                else
                {
                    viewModel.NozzleInformation.ForEach(x =>
                    {
                        x.Modificadopor = User.Identity.Name;
                        x.Fechamodificacion = DateTime.Now;
                    });
                }

                var result = await this._nozzleInformationService.SaveRecordNozzleInformation(viewModel);

                if (result.Code == 1)
                {
                    result.Description = viewModel.OperationType == true ? "Registro exitoso" : "Actualización exitosa";
                }

                return Json(new { response = result });
            }
            catch(Exception e)
            {
                return null;
            }
        
        }

    }
    public class pruebaa
    {
        public  int value { get; set; }
        public  string name { get; set; }
    }
    public class Node
    {
        public Node LeftChild { get; private set; }
        public Node RightChild { get; private set; }

        public Node(Node leftChild, Node rightChild)
        {
            this.LeftChild = leftChild;
            this.RightChild = rightChild;
        }

        public int Height(Node node )
        {
            int distL = 0;

            if(node!= null)
            {
                return 0;
            }

            if (node.LeftChild != null)
            {
                distL++;
                distL = distL + Height(node.LeftChild );
            }

            int distR= 0;
            if (node.RightChild!= null)
            {
                distR++;
                distR = distR + Height(node.RightChild);
            }

            if(distR > distL)
            {
                return distR;
            }
            else
            {
                return distL;
            }

        }
    }

}
