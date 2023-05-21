using Microsoft.AspNetCore.Authentication;

using SPL.Domain;
using SPL.WebApp.Domain.DTOs.ProfileSecurity;
using SPL.WebApp.Domain.Services.ProfileSecurity;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SPL.WebApp.Services
{
    public class CustomClaimsPrincipalFactory : ICustomClaimsPrincipalFactory
    {
        private readonly IProfileSecurityService _profileClientService;
        public CustomClaimsPrincipalFactory(IProfileSecurityService profileClientService)
        {
            _profileClientService = profileClientService;
        }
        public Task<List<Claim>> TransformAsync(string idUser)
        {
            List<Claim> listClaims = new List<Claim>();
            try
            {
                ApiResponse<List<UserPermissionsDTO>> listPermissions = this._profileClientService.GetPermissionUsers(idUser).Result;

    

                foreach (var item in listPermissions.Structure)
                {
                    listClaims.Add(new Claim(ClaimTypes.Role, item.ClaveOpcion));
                }

                return Task.FromResult(listClaims);
            }
            
            catch (System.Exception)
            {
                return Task.FromResult(listClaims);

                //throw;
            }

         
        
    }
 }
}
