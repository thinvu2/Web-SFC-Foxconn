using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Ulist.Model
{
    public class Data
    {
        public string unique_name { get; set; }
        public string password { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
    }

    public class Root
    {
        public Data data { get; set; }
    }

}