using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SN_API.Models
{
    public class QMElement
    {
        public string database_name { get; set; }
        public string type { get; set; }
        public string dateFrom { get; set; }
        public string dateFromOrigi { get; set; }
        public string timeFrom { set; get; }
        public string dateTo { get; set; }
        public string timeTo { set; get; }
        public string value { get; set; }
        public string moState { get; set; }
        public bool iDate { get; set; }
        public bool iShowAllpart { get; set; }
        public bool iLine { get; set; }
        public bool iSection { get; set; }
        public bool iGroup { get; set; }
        public bool iModelName { get; set; }
        public bool iModelSerial { get; set; }
        public bool iMoNumber { get; set; }
        public bool iRepassQTY { get; set; }
        public bool iRefailQTY { get; set; }
        public bool iTFailPassQTY { get; set; }
        public bool iTFailFailQTY { get; set; }
        public bool iSerialNumber { get; set; }
        public bool iTestCode { get; set; }
        public bool iLocationCode { get; set; }
        public bool iLotNo { get; set; }
        public bool iSide { get; set; }
        public bool iErrorCode { get; set; }
        public bool iErrorDesc { get; set; }
        public bool iDefectQty { get; set; }
        public bool iDPMO { get; set; }
        public string testCode { get; set; }
        public string reasonCode { get; set; }
        public string Desc { get; set; }
        public string dutyType { get; set; }
        public string qcSample { get; set; }
        public string lastLotNo { get; set; }
        public int defectType { get; set; }


        public List<Type> listModel;
        public List<Type> listMo;
        public List<Type> listLine;
        public List<Type> listGroup;
    }
    public class Type
    {
        public string VALUE { get; set; }
    }
}