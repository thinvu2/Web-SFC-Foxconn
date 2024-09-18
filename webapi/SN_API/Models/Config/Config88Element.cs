using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Controllers.Config
{
    public class Config88Element
    {
       
            public string ID { get; set; }
            public string EMP { get; set; }
            public string TABLE_NAME { get; set; }
            public string database_name { get; set; }
            public string PK_COLUMN { get; set; }
            public string TYPE { get; set; }
            public string DATA1 { get; set; }
            public string DATA2 { get; set; }
            public string DATA3 { get; set; }
            public string DATA4 { get; set; }
            public string DATA5 { get; set; }
        
    }
    public class Config88_Setup
    {
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string GROUP_NAME { get; set; }

        public string MODEL_NAME_S { get; set; }
        public string GROUP_NAME_S { get; set; }
        public string PORT { get; set; }

        public string FORMAT { get; set; }
        public string ACTION { get; set; }
        public Int32 LENGTH { get; set; }
        public string INPUT { get; set; }

    }

    public class Config88_Setup_Copy
    {
        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string GROUP_NAME { get; set; }
        public string MODEL_NAME_NEW { get; set; }


    }

    public class Config88_Setup_Search
    {
        // public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string GROUP_NAME { get; set; }



    }

    public class Config88_Combine
    {


        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string MO_TYPE { get; set; }
        public string GROUP_NAME { get; set; }

        public string MODEL_NAME_S { get; set; }
        public string MO_TYPE_S { get; set; }
        public string GROUP_NAME_S { get; set; }

        public string TABLE_NAME { get; set; }
        public string TABLE_BAK { get; set; }
        public Int32 PASS_LENGTH { get; set; }
        public Int32 FAIL_LENGTH { get; set; }
        public string ERROR_LENG { get; set; }
        public string FIELD_DESC { get; set; }
        public string COLUMN_NAME { get; set; }
        public Int32 SUB_POSTION { get; set; }
        public Int32 SUB_LENG { get; set; }
        public string STORAGES { get; set; }
        public string CHECK_DUP { get; set; }
        public string WHERE_FIELD { get; set; }
        public string CHECK_SP { get; set; }
        public string DATE_CREATE { get; set; }
        public string EMP_NO { get; set; }
        public string DATA1 { get; set; }
        public string DATA2 { get; set; }
        public string DATA3 { get; set; }
        public string DATA4 { get; set; }
        public string DATA5 { get; set; }

    }
    public class Config88_Combine_Copy
    {

        public string EMP { get; set; }
        public string database_name { get; set; }
        public string MODEL_NAME { get; set; }
        public string MO_TYPE { get; set; }
        public string GROUP_NAME { get; set; }
        public string MODEL_NAME_NEW { get; set; }


    }

    public class Config88_Combine_add
    {

        public string EMP { get; set; }
        public string database_name { get; set; }
        public string Data_Old { get; set; }
        public string Data_New { get; set; }
        public string Model_Old { get; set; }
        public string Action_type { get; set; }
      
    }
}