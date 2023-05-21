using AutoMapper;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;

using SPL.Domain;
using SPL.WebApp.Domain.DTOs.ProfileSecurity;
using SPL.WebApp.Domain.Services.ProfileSecurity;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPL.WebApp.Controllers
{
  
  
    public class DrawerUserController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        //private readonly IHttpClientFactory _clientFactory;

        private readonly IProfileSecurityService _profileClientService;
        public DrawerUserController(

        IMapper mapper, ITokenAcquisition tokenAcquisition, IProfileSecurityService profileClientService
                           )
        {

            this._profileClientService = profileClientService;
            this._tokenAcquisition = tokenAcquisition;
        }


        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(principal.Identity.Name).Result;

                List<Claim> claims = new List<Claim>();
                string permission = "";
                foreach (var item in listPermissions.Structure)
                {
                    permission = permission + ";" + item.ClaveOpcion;


                }


                ClaimsIdentity claimsIdentity = new ClaimsIdentity();

                var claimType = "permission";

                if (!principal.HasClaim(claim => claim.Type == claimType))
                {
                    claimsIdentity.AddClaim(new Claim(claimType, permission));
                }

                principal.AddIdentity(claimsIdentity);
                return Task.FromResult(principal);
            }

            catch (System.Exception)
            {
                return Task.FromResult(principal);

                //throw;
            }


        }

    }
}
