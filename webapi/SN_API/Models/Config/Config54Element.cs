using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config54Element
    {
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string CARTON_QTY { get; set; }
        public string CARTON_LABEL_WEIGHT { get; set; }
        public string PRODUCT_DESC { get; set; }
        public string ACTION_TYPE { get; set; }
    }
    public class SetupModelRework
    {
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string MODEL_DESC { get; set; }
        public string FROM_GROUP { get; set; }
        public string TO_GROUP { get; set; }
        public string ACTION_TYPE { get; set; }
        public string ID { get; set; }
      //  public string ACTION_TYPE { get; set; }
    }
    
}