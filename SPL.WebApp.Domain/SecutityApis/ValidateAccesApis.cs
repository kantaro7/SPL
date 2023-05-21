using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SPL.WebApp.Domain.SecurityApis
{
    public class ValidateAccesApis : IValidateAccesApis
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ITokenAcquisition _tokenAcquisition;
      

        public ValidateAccesApis(IConfiguration config, IHttpClientFactory clientFactory,
            ITokenAcquisition tokenAcquisition)
        {
            this._configuration = config;
            _clientFactory = clientFactory;
            _tokenAcquisition = tokenAcquisition;

           
        }

        public async Task<HttpClient> getTokenSesionAsync(HttpClient httpClient, int microservices)
        {

            try
            {
                string[] initialScopes = new string[] { "" };
                if (microservices == (int)Enums.MicroservicesEnum.spldesigninformation)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenDesigninformation")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.splmasters)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenMasters")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.splsidco)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenSidco")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.splreports)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenReports")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.spltests)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenTest")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.splconfiguration)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenConfiguration")?.Split(' ');
                }
                else if (microservices == (int)Enums.MicroservicesEnum.splsecurity)
                {
                    initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenSecurity")?.Split(' ');
                }
  
         
            var token = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

            httpClient.DefaultRequestHeaders.Authorization =
                     new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            return httpClient;
            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                throw new ArgumentNullException(paramName: "Error", message: $"{e.GetType()}: {e.Message}{Environment.NewLine}Stacktrace:{Environment.NewLine}{e.StackTrace}{Environment.NewLine}{Environment.NewLine}Cierre sesión y vuelva a iniciar sesión para solucionar el problema");

            }
            catch (Exception ex)
            {

                throw new ArgumentNullException(paramName: "Error", message: ex.Message);
            }
        }



        public async Task<string> getTokenSesionGatewayAsync(int microservices)
        {
            string tokes ="";
            try
            {
                string[] initialScopes = new string[] { "" };
              
                initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenDesigninformation")?.Split(' ');

                var gettoken = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

                tokes = gettoken;


                initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenMasters")?.Split(' ');

                gettoken = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

                tokes = tokes + " " + gettoken;

                initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenReports")?.Split(' ');

                gettoken = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

                tokes = tokes + " " + gettoken;

                initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenTest")?.Split(' ');

                gettoken = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

                tokes = tokes + " " + gettoken;

                initialScopes = _configuration.GetValue<string>("CallApi:ScopeForAccessTokenConfiguration")?.Split(' ');

                gettoken = await _tokenAcquisition.GetAccessTokenForUserAsync(initialScopes).ConfigureAwait(false);

                tokes = tokes + " " + gettoken;


             

                return tokes;
            }
            catch (MicrosoftIdentityWebChallengeUserException e)
            {
                throw new ArgumentNullException(paramName: "Error", message: $"{e.GetType()}: {e.Message}{Environment.NewLine}Stacktrace:{Environment.NewLine}{e.StackTrace}{Environment.NewLine}{Environment.NewLine}Cierre sesión y vuelva a iniciar sesión para solucionar el problema");

            }
            catch (Exception ex)
            {

                throw new ArgumentNullException(paramName: "Error", message: ex.Message);
            }
        }

        public static string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }



        //public static string Compress(byte[] bytes)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        using (var gzipStream = new GZipStream(memoryStream, CompressionLevel.Optimal))
        //        {
        //            gzipStream.Write(bytes, 0, bytes.Length);
        //        }
        //        return Convert.ToBase64String(memoryStream.ToArray());
        //    }
        //}

        //public static string Compress(string uncompressedString)
        //{
        //    byte[] compressedBytes;

        //    using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
        //    {
        //        using (var compressedStream = new MemoryStream())
        //        {
        //            // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
        //            // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
        //            // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
        //            using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.SmallestSize, true))
        //            {
        //                uncompressedStream.CopyTo(compressorStream);
        //            }

        //            // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
        //            compressedBytes = compressedStream.ToArray();
        //        }
        //    }

        //    return Convert.ToBase64String(compressedBytes);
        //}

    }
}
