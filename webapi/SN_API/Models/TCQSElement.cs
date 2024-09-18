using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{    
     public class TCQSElement
    {
        public string database_name { get; set; }
        public string type { get; set; }
        public string dateFrom { get; set; }
        public string timeFrom { set; get; }
        public string dateTo { get; set; }
        public string timeTo { set; get; }
        public string value { get; set; }
        public bool iOperate { set; get; }
        public bool iDate { set; get; }
        public bool iModelName { set; get; }
        public bool iVersionCode { set; get; }
        public bool iMoNumber { get; set; }
        public bool iLineName { set; get; }
        public bool iGroupName { set; get; }
        public bool iStationName { set; get; }
        public bool iByHour { set; get; }

        public List<Type> listModel;
        public List<Type> listMo;
        public List<Type> listLine;
        public List<Type> listGroup;
    }    
}