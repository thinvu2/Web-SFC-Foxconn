﻿using System.Collections.Generic;

namespace SN_API.Models
{
    public class OutputAps
    {
        public string status { get; set; }
        public string message { get; set; }
        public object report_list { get; set; }
    }

    public class Detail_NEW
    {
        public string mo_no { get; set; }
        public int qty { get; set; }
    }

    public class ReportList
    {
        public int serial_number { get; set; }
        public string station { get; set; }
        public int total_quantity { get; set; }
        public List<Detail_NEW> detail { get; set; }
    }
}