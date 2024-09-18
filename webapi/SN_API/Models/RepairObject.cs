using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class RepairObject
    {
        public string database { set; get; }
        public string date_from { set; get; }
        public string date_to { set; get; }
        public string option { set; get; }
        public List<Type> group_list { set; get; }
        public List<Type> model_list { set; get; }
        public List<Type> group_by { set; get; }
    }
    //public class group_list
    //{
    //    public string VALUE { set; get; }
    //}   
}