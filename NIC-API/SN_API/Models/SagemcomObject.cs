using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class SagemcomObject
    {
        public string database { set; get; }
        public string date_from { set; get; }
        public string date_to { set; get; }
        public string option { set; get; }
        public string timeFrom { set; get; }
        public string timeTo { set; get; }
        public List<Type> group_mo { set; get; }
        public List<Type> group_model { set; get; }
    }
    //public class group_list
    //{
    //    public string VALUE { set; get; }
    //}   
}