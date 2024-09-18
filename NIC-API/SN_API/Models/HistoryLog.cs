using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class HistoryLog
    {
        public string database_name { get; set; }
        public string model_name { get; set; }
        public string group_name { get; set; }
        public string action_type { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
    }
}