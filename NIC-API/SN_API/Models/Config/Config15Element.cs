using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config15Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string TRAY_QTY { get; set; }
        public string CARTON_QTY { get; set; }
        public string PALLET_QTY { get; set; }
        public string CREATE_NAME { get; set; }
        public string COO { get; set; }
        public string SN_QTY { get; set; }
        public string LABEL_QTY { get; set; }
        public string PACK_FLAG { get; set; }
        public bool IS_CHECKMAC { get; set; }
        public bool IS_ORDER_BOX { get; set; }
        public bool IS_ADDTRAY { get; set; }
    }
}