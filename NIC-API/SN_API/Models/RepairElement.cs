using System.Collections.Generic;

namespace SN_API.Models
{
    public class RepairElement
    {
        public string database_name { get; set; }
        public string type { get; set; }
        public string dateFrom { get; set; }
        public string timeFrom { set; get; }
        public string dateTo { get; set; }
        public string timeTo { set; get; }          
    }
}