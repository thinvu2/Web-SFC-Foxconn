using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models.Config
{
    public class Config7Element
    {
        public string ID { get; set; }
        public string database_name { get; set; }
        public string EMP_NO { get; set; }
        public string EMP_NAME { get; set; }
        public string CLASS_NAME { get; set; }
        public string EMP_PASS { get; set; }
        public string EMP_BC { get; set; }
        public string PRG_NAME { get; set; }
        public string FUN { get; set; }
        public string PASSW { get; set; }
        public string PRIVILEGE { get; set; }
        public string selectedItems { get; set; }
        public string EMP_NO_NAME { get; set; }
        public string EMP_COPYDF { get; set; }
        public string EMP_COPYNOTDF { get; set; }
        public string EMP { get; set; }
        public string SPRG_NAME { get; set; }
        public string DPRG_NAME { get; set; }
        public string EMP_PWD_PASS { get; set; }
        public string DefineAppList { get; set; }
        public string SELECT_CHOOSE { get; set; }
        public DateTime QuitDate { get; set; }
        public string value { get; set; }
        public string LISTINPUTEMP { get; set; }
        public string valueInput { get; set; }
        public List<Group> LISTEMP;
        public List<Group> LISTGROUPDF;
        public List<Group> LISTGROUPNOTDF;
        public List<Group> ListDataAms;
        public List<Group> listGroup;
  
    }
    public class Group
    {
        public string VALUE { get; set; }
        public string PRG_NAME { get; set; }
        public string FUN { get; set; }
        public string PASSW { get; set; }
        public bool isSelected{ get; set; }
        public string PRIVILEGE { get; set; }
        public string EMP_NO { get; set; }
        public string EMP { get; set; }
        public string AP_GROUP { get; set; }
        public string ROW_ID { get; set; }

    }

}