using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config76Element
    {
        public string ID { get; set; }
        //public string EMP { get; set; }
        //public string database_name { get; set; }
        //public string MODEL_TYPE { get; set; }
        //public string TYPE_NAME { get; set; }
        //public string TYPE_FLAG { get; set; }
        //public string TYPE_DESC { get; set; }
        //public string EMP_NO { get; set; }
        //public string CREATE_DATE { get; set; }
        //public string DATA { get; set; }

   
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MO_NUMBER { get; set; }
        public string GROUP_NAME { get; set; }
        public string STATUS { get; set; }
        //   public string EMP { get; set; }
        public string CREATE_DATE { get; set; }
    }

    public class ConfigFAElement
    {


       
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MO_NUMBER { get; set; }
        public string FA_NUMBER { get; set; }
        public string FA_VERSION { get; set; }
        public string FA_NUMBER1 { get; set; }
        public string FA_VERSION1 { get; set; }
        public string FA_NUMBER2 { get; set; }
        public string FA_VERSION2 { get; set; }
        public string FA_NUMBER3 { get; set; }
        public string FA_VERSION3 { get; set; }
        public string FA_NUMBER4 { get; set; }
        public string FA_VERSION4 { get; set; }

    }  //   public string EMP { get; set; }


}