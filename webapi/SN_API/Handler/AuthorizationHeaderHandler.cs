using Newtonsoft.Json;
using SN_API.Handler.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SN_API.Models.Ulist.Model;
using System.Web.Script.Serialization;

namespace SN_API.Handler
{
    public class AuthorizationHeaderHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                AuthenticationHeaderValue authorization = request.Headers.Authorization;
                if (authorization == null)
                {
                    return base.SendAsync(request, cancellationToken);
                }
                string token = authorization.Parameter;
                var _payload = JwtToken.GetPayLoad(token);

                string _output = JsonConvert.SerializeObject(_payload);
                var _unique_name = _payload["username"].ToString();
                var _password = _payload["password"].ToString();

                if (_payload != null)
                {
                   // string cc = a.unique_name;
                    var identity = new GenericIdentity(_unique_name);
                    SetPrincipal(new GenericPrincipal(identity, null));
                }
                return base.SendAsync(request, cancellationToken);
            }
            catch(Exception ex)
            {
                return base.SendAsync(request, cancellationToken);
            }
        }
        private static void SetPrincipal(IPrincipal principal)
        {
            // setting.   
            Thread.CurrentPrincipal = principal;
            // Verification.   
            if (HttpContext.Current != null)
            {
                // Setting.   
                HttpContext.Current.User = principal;
            }
        }
    }
}