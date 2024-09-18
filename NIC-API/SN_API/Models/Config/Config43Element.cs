using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config43Element
    {
        public string ID { get; set; }
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string VERSION_CODE { get; set; }
        public string MO_TYPE { get; set; }
        public int SCAN_SEQ { get; set; }
        public string CUSTSN_NAME { get; set; }
        public List<custsnname> listCUSTSN_NAME { get; set; }
    }
    public class custsnname
    {
        public string VALUE { get; set; }
    }
}