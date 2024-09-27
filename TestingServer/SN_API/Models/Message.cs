using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class Request
    {
        public string Action { get; set; }
        public string SN { get; set; }
        public string ModelName { get; set; }
        public string WorkStation { get; set; }
        public string PCName { get; set; }
        public string LineName { get; set; }
        [DefaultValue("SFIS")]
        [Length(1, 15)]
        public string EmpNo { get; set; }
        public string TestFormatName { get; set; }
        [DefaultValue("Null")]
        [Length(0, 500)]
        public string SendData { get; set; }
        [DefaultValue("Null")]
        [Length(0, 30)]
        public string TestResult { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class Response
    {
        public string Action { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

    }
}