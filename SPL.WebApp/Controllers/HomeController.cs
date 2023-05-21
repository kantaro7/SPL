namespace SPL.WebApp.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Graph;
    using Microsoft.Identity.Web;

    using SPL.Domain;
    using SPL.WebApp.Domain.DTOs.ProfileSecurity;
    using SPL.WebApp.Domain.Services.ProfileSecurity;
    using SPL.WebApp.Services;

    public class HomeController : Controller
    {
        private readonly ITokenAcquisition _tokenAcquisition;
        //private readonly IHttpClientFactory _clientFactory;

        private readonly IProfileSecurityService _profileClientService;
        private readonly ICustomClaimsPrincipalFactory _customClaims;
        #region imgUserDefault
        private readonly string _imageUserDefault = "iVBORw0KGgoAAAANSUhEUgAAAgAAAAIACAMAAADDpiTIAAAABGdBTUEAALGPC/xhBQAAAAFzUkdCAK7OHOkAAAMAUExURUxpcf///39/f6qqqr+/v8zMzKqqqra2tr+/v6mpqbKysrm5ub+/v7CwsLa2tru7u6+vr7S0tLi4uLu7u7Kysra2trm5ubGxsbS0tLe3t7q6urOzs7a2tri4uLKysrS0tLe3t7m5ubS0tLa2tri4uLOzs7W1tbe3t7i4uLS0tLa2tre3t7Ozs7W1tba2tri4uLS0tLa2tre3t7S0tLW1tba2tri4uLS0tLa2tre3t7S0tLW1tba2tre3t7S0tLa2tre3t7S0tLW1tba2tre3t7W1tba2tre3t7S0tLW1tba2tre3t7W1tba2tre3t7S0tLW1tba2tre3t7W1tba2tre3t7S0tLW1tba2tre3t7W1tba2tra2trS0tLW1tba2tre3t7W1tba2tra2trW1tbW1tba2tre3t7W1tba2tra2trW1tbW1tba2tre3t7W1tba2tra2trW1tbW1tba2tre3t7W1tba2tra2trW1tbW1tba2tre3t7W1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tbW1tba2trW1tbW1tba2tra2trW1tba2tvhlUZAAAAD/dFJOUwABAgMEBQYHCAkKCwwNDg8QERITFBUWFxgZGhscHR4fICEiIyQlJicoKSorLC0uLzAx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";

        #endregion
        public HomeController(ITokenAcquisition tokenAcquisition, IProfileSecurityService profileClientService, ICustomClaimsPrincipalFactory customClaims)
        {

            _profileClientService = profileClientService;
            _tokenAcquisition = tokenAcquisition;
            _customClaims = customClaims;
        }
        //[Authorize]
        //[AuthorizeForScopes(Scopes = new string[] { "User.ReadBasic.All", "User.Read", "api://14f15439-78be-4e6e-a897-c07e9b6a7a76/acces_as_user" })]
    
        public async Task<ActionResult> Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                ApiResponse<List<UsersDTO>> listUser = null;


                try
                {
                    listUser = _profileClientService.GetUsers(User.Identity.Name).Result;

                }
                catch (MicrosoftIdentityWebChallengeUserException e)
                {
                    return View("~/Views/PageConstruction/Error.cshtml");


                }
                catch (Exception ex)
                {

                    return View("~/Views/PageConstruction/Error.cshtml");

                }


                if (listUser.Structure.Count == 0)
                {

                    string token = _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { "User.ReadBasic.All", "User.Read" }).Result;

                    GraphServiceClient graphClient = new("https://graph.microsoft.com/v1.0/", new DelegateAuthenticationProvider(request =>
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                        return Task.CompletedTask;

                    }));
                    User currentUser = null;

                    currentUser = graphClient.Me.Request().GetAsync().Result;

                    List<UsersDTO> list = new()
                    {
                        new UsersDTO() { NombreIdentificador = User.Identity.Name, Nombre = currentUser.DisplayName }
                    };
                    ApiResponse<long> getStatusLoad = _profileClientService.SaveUsers(list).Result;

                }

                ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

                if (listPermissions.Structure.Count == 0)
                {
                    return View("~/Views/PageConstruction/PermissionDenied.cshtml");
                }
            }

            return View();
        }

        //public async Task<User> GetGraphApiUser()
        //{
        //    var graphclient = await GetGraphClient(
        //        new string[] { "User.ReadBasic.All", "user.read", "api://414f15439-78be-4e6e-a897-c07e9b6a7a76/acces_as_user" })
        //       .ConfigureAwait(false);

        //    return await graphclient.Me.Request()
        //       .GetAsync().ConfigureAwait(false);
        //}

        //private async Task<GraphServiceClient> GetGraphClient(string[] scopes)
        //{
        //    var token = await _tokenAcquisition.GetAccessTokenForUserAsync(
        //     scopes).ConfigureAwait(false);

        //    var client = _clientFactory.CreateClient();
        //    client.BaseAddress = new Uri("https://graph.microsoft.com/v1.0/");
        //    client.DefaultRequestHeaders.Accept.Add(
        //        new MediaTypeWithQualityHeaderValue("application/json"));

        //    GraphServiceClient graphClient = new GraphServiceClient(client)
        //    {
        //        AuthenticationProvider = new DelegateAuthenticationProvider(
        //        async (requestMessage) =>
        //        {
        //            requestMessage.Headers.Authorization =
        //                new AuthenticationHeaderValue("bearer", token);

        //        })
        //    };

        //    graphClient.BaseUrl = "https://graph.microsoft.com/v1.0/";
        //    return graphClient;
        //}

        public IActionResult GetPermissions()
        {
            ApiResponse<List<UserPermissionsDTO>> listPermissions = _profileClientService.GetPermissionUsers(User.Identity.Name).Result;

            return PartialView("_Menu", listPermissions.Structure);
        }

        public ActionResult GetImage()
        {
            string token = _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { "User.ReadBasic.All", "User.Read" }).Result;

            GraphServiceClient graphClient = new("https://graph.microsoft.com/v1.0/", new DelegateAuthenticationProvider(request =>
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return Task.CompletedTask;

            }));

            byte[] photoByte = null;
            try
            {
                User currentUser = null;

                currentUser = graphClient.Me.Request().GetAsync().Result;

                try
                {
                    // Get user photo
                    using Stream photoStream = graphClient.Me.Photo.Content.Request().GetAsync().Result;
                    photoByte = ((MemoryStream)photoStream).ToArray();
                    //ViewData["Photo"] = Convert.ToBase64String(photoByte);
                }
                catch (Exception)
                {
                    //Console.WriteLine($"{pex.Message}");
                    photoByte = null;
                }

                //ViewData["Me"] = currentUser;
                //return View();
                return photoByte == null ? File(Convert.FromBase64String(_imageUserDefault), "image/png") : (ActionResult)File(photoByte, "image/png");
            }
            catch (Exception)
            {
                return photoByte != null && photoByte.Length != 0 ? File(photoByte, "image/png") : (ActionResult)null;
            }
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error() => View();
    }
}
