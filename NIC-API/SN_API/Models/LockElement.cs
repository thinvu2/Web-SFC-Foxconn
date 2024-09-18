using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class LockElement
    {
        public string database_name { get; set; }
        public string value { set; get; }
        public string customer { set; get; }
        public string type { get; set; }
        public List<Type> listModel;
        public List<Type> listGroup;
    }
}