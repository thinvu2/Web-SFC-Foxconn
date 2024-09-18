using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SN_API.Handler.JWT;
using SN_API.Models;
using SN_API.Models.Ulist.Response;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace SN_API.Class
{
    public class AuthorizationHeaderParameterOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, System.Web.Http.Description.ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "access token",
                required = false,
                type = "string",
                @default = "Bearer "
            });
        }
    }
}