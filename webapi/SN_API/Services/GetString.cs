using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Services
{
    public class GetString
    {
        public static string UI = StringConnect("10.224.134.98", "1527", "SFCODB", "HOGAN", "sfcsystem2014#!");
        public static string CQSFC = StringConnect("10.245.212.61", "1903", "SFCODB", "HOGAN", "cqtovnsfc");
        public static string CPEII = StringConnect("10.224.81.34", "1521", "VNSFC", "SFIS1", "VNSFIS2014#!");
        public static string TEST = StringConnect("10.220.94.68", "1521", "SFCODB", "SFIS1", "dog2007#!");

        //public static string CPEII = StringConnect("10.224.81.72", "1521", "VNSFCT", "SFIS1", "VNSFIS2014#!");
        public static string ALLPART = StringConnect("10.220.81.99", "1521", "VNAP", "AP2", "NSDAP2LOGPD0522");
        public static string SFO = StringConnect("10.224.130.40", "1521", "VNSFC", "SFIS1", "VNSFIS2014#!");
        public static string CPEI = StringConnect("10.224.81.33", "1521", "VNSFC", "SFIS1", "VNSFIS2014#!");
        public static string BN3 = StringConnect("10.225.35.10", "1521", "VNSFC", "SFIS1", "VNSFIS2014#!");
        public static string NIC = StringConnect("10.220.96.200", "1521", "VNSFC", "SFIS1", "VNSFIS2014#!");
        public static string ROKU = StringConnect("10.224.140.198", "1527", "SFCODB", "SFIS1", "hift2013!");
        public static string UBEE = StringConnect("10.220.99.135", "1527", "vnsfc", "SFIS1", "dog2007#!");
        public static string CQ = StringConnect("10.245.212.61", "1903", "SFCODB", "VNSFC", "foxconnvnsfc");
        public static string CPEII_RMA = StringConnect("10.224.69.80", "1521", "VNRMA", "SFIS1", "VNSFIS2014#!");
        public const string tempConnectionString = "DATA SOURCE=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1})))(CONNECT_DATA = (SERVICE_NAME = {2})));User ID={3}; Password={4};Connection Timeout=120;";

        public static string StringConnect(string host_, string port_, string service_name_, string username_, string password_)
        {
            return String.Format(tempConnectionString, host_, port_, service_name_, username_, password_);
        }
        public Dictionary<string, string> Get()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>()
            {
                {"ALLPART", ALLPART },
                {"UI",UI },
                {"CQSFC",CQSFC },
                {"TEST",TEST },             
                {"SFO",SFO},
                {"CPEII",CPEII },
                {"CPEI",CPEI },
                {"BN3",BN3 },
                {"NIC",NIC },
                {"ROKU",ROKU },
                {"UBEE",UBEE },
                {"CQ",CQ },
                {"CPEII_RMA",CPEII_RMA },
            };
            return dictionary;
        }
    }
}