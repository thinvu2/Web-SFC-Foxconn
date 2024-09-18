using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class BonePileElement
    {
        public string database_name { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }

        public List<Type> listModel;
    }
}