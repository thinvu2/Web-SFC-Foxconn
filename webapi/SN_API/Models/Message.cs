using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Message
    { 
        public string MessageContent { get; set; }
        public string HostIP { get; set; }
        public string GroupKey { get; set; }
        public string ApiLink { get; set; }
        public string dbKey { get; set; }

    }
}