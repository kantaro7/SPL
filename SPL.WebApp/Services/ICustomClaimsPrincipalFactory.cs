namespace SPL.WebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using SPL.WebApp.Domain.DTOs;
    using SPL.WebApp.Domain.Enums;
    using Telerik.Web.Spreadsheet;

    public interface ICustomClaimsPrincipalFactory
    {
        public Task<List<Claim>> TransformAsync(string idUser);

    }
}
