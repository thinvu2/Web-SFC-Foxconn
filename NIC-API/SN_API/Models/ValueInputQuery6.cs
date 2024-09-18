using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class ValueInputQuery6
    {
        public string database { set; get; }
        public string table { set; get; }
        public List<ListInput> list_input { set; get; }
        public List<ListGroupName> list_group_name { set; get; }
        public string field { set; get; }
        public string date_from { set; get; }
        public string date_to { set; get; }

        public class ListInput
        {
            public string input { set; get; }
        }
        public class ListGroupName
        {
            public string input { set; get; }
        }
    }
}