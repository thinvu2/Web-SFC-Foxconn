using SN_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using ZXing;

namespace SN_API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class Query6Controller : ApiController
    {
        // GET: Query6
        [System.Web.Http.Route("CheckRouteExist")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> CheckRouteExist(RouteItem model)
        {
            StringBuilder builder = new StringBuilder();

            string SQL = $"select route_code,route_name from SFIS1.C_ROUTE_NAME_T where route_code in (select route_code from sfism4.R105 where mo_number in (select mo_number from sfism4.R107 where serial_number = '{model.value}') and rownum=1)";

            builder.Append(SQL);

            string queryString = builder.ToString();

            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
        [System.Web.Http.Route("GetAllGroup")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetAllGroup(RouteItem model)
        {
            StringBuilder builder = new StringBuilder();

            string SQL = $"select distinct A.group_name VALUE from SFIS1.C_GROUP_CONFIG_T  A ORDER BY A.GROUP_NAME ASC";

            builder.Append(SQL);

            string queryString = builder.ToString();

            DataTable dtCheck = DBConnect.GetData(queryString, model.database_name);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }

        public Image Base64ToImage(string base64String)
        {
            Image image = null;
            // Convert Base64 String to byte[]
            try
            {
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);

            }
            catch (Exception ex)
            {
                return image;
            }
            return image;
        }

        public Bitmap Base64StringToBitmap(string base64String)
        {
            Bitmap bmpReturn = null;
            try
            {
                string converted = base64String.Replace("data:image/jpeg;base64,", "");
                byte[] byteBuffer = Convert.FromBase64String(converted);
                MemoryStream memoryStream = new MemoryStream(byteBuffer)
                {
                    Position = 0
                };
                bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
                memoryStream.Close();
                memoryStream = null;
                byteBuffer = null;
            }
            catch (Exception)
            {
                return bmpReturn;
            }
            return bmpReturn;
        }



        //public string Decode()
        //{
        //    using (var image = new Image<Gray, Byte>(_bitmap))
        //    {
        //        using (Image<Gray, byte> bw = image.Convert(b => (byte)((b < 128) ? 0 : 255)))
        //        {
        //            var reader = new BarcodeReader();
        //            Result result = reader.Decode(bw.Bitmap);
        //            if (result == null) throw new Exception("Баркод не распознан");
        //            byte[] bytes = Convert.FromBase64String(result.Text);
        //            int count = Marshal.SizeOf(typeof(BinaryData));
        //            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        //            var binaryData =(BinaryData)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(BinaryData));
        //            handle.Free();
        //            ArchiverIndex = binaryData.ArchiverIndex;
        //            GammaIndex = binaryData.GammaIndex;
        //            MixerIndex = binaryData.MixerIndex;
        //            EccIndex = binaryData.EccIndex;
        //            ExpandSize = binaryData.ExpandSize;
        //            EccCodeSize = binaryData.EccCodeSize;
        //            EccDataSize = binaryData.EccDataSize;
        //            MaximumGamma = (binaryData.MaximumGamma == 0);
        //            Key = Encoding.Default.GetString(bytes.ToList().GetRange(count, bytes.Length - count).ToArray());
        //            return ToString();
        //        }
        //    }

        //}
        private static readonly List<BarcodeFormat> Fmts = new List<BarcodeFormat> { BarcodeFormat.All_1D };

        public static bool DecodeImg(Bitmap img)
        {
            BarcodeReader reader = new BarcodeReader
            {
                AutoRotate = true,
                TryInverted = true,
                Options =
            {
                PossibleFormats = Fmts,
                TryHarder = true,
                ReturnCodabarStartEnd = true,
                PureBarcode = false
            }
            };
            Result result = reader.Decode(img);

            if (result != null)
            {
                Console.WriteLine(result.BarcodeFormat);
                Console.WriteLine(result.Text);
                return true;
            }

            Console.Out.WriteLine("Raté");
            return false;
        }

        [System.Web.Http.Route("ScanBarcode")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> ScanBarcode(Q6Element model)
        {
            string valueScan = "";
            StringBuilder builder = new StringBuilder();
            Bitmap imgScan = Base64StringToBitmap(model.value);

            IBarcodeReader reader = new BarcodeReader();
            // load a bitmap
            var barcodeBitmap = imgScan;
            // detect and decode the barcode inside the bitmap
            var result = reader.Decode(barcodeBitmap);
            // do something with the result
            if (result == null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "null", barcode = "null" });
            }
            valueScan = result.Text;
            //return Request.CreateResponse(HttpStatusCode.OK, new { result = "null", });
            string SQL = $"select SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from SFISM4.R107 where SERIAL_NUMBER ='{valueScan}' OR  SHIPPING_SN ='{valueScan}' OR SHIPPING_SN2 ='{valueScan}'";
            builder.Append(SQL);

            string queryString = builder.ToString();

            DataTable dtCheck = DBConnect.GetData(queryString, model.database);



            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail", barcode = "null" });
            }
            else
            {
                string subSQL = $"select SERIAL_NUMBER,SECTION_FLAG,MO_NUMBER,MODEL_NAME,TYPE,VERSION_CODE,LINE_NAME,SECTION_NAME,GROUP_NAME,WIP_GROUP,STATION_NAME,LOCATION,STATION_SEQ,ERROR_FLAG,TO_CHAR (IN_STATION_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_STATION_TIME,TO_CHAR (IN_LINE_TIME, 'yyyy/mm/dd hh24:mi:ss') IN_LINE_TIME,OUT_LINE_TIME,SHIPPING_SN,WORK_FLAG,FINISH_FLAG,ENC_CNT,SPECIAL_ROUTE,PALLET_NO,CONTAINER_NO,QA_NO,QA_RESULT,SCRAP_FLAG,NEXT_STATION,CUSTOMER_NO,BOM_NO,BILL_NO,TRACK_NO,PO_NO,KEY_PART_NO,CARTON_NO,WARRANTY_DATE,REWORK_NO,REPAIR_CNT,EMP_NO,PO_LINE,PALLET_FULL_FLAG,PMCC,GROUP_NAME_CQC,MO_NUMBER_OLD,ERP_MO,ATE_STATION_NO,MSN,IMEI,JOB,MCARTON_NO,SO_NUMBER,SO_LINE,STOCK_NO,TRAY_NO,SHIP_NO,SHIPPING_SN2 from SFISM4.R117 where SERIAL_NUMBER ='{dtCheck.Rows[0]["SERIAL_NUMBER"]}' ORDER BY IN_STATION_TIME ASC ";
                DataTable subDtCheck = DBConnect.GetData(subSQL, model.database);
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck, data1 = subDtCheck, barcode = valueScan });
            }
        }

        [System.Web.Http.Route("GetByClick")]
        [System.Web.Http.HttpPost]
        public async Task<HttpResponseMessage> GetByClick(Q6Element model)
        {
            StringBuilder builder = new StringBuilder();

            string SQL = "";
            if (model.field == "MODEL_NAME")
            {
                SQL = $"select * from SFIS1.C_SMO_TYPE_T where model_name ='{model.value}' order by group_name,port_type asc";
            }
            else if (model.field == "MO_NUMBER")
            {
                SQL = $"select * from sfism4.R105 where mo_number = '{model.value}'";
            }
            else if (model.field == "SERIAL_NUMBER")
            {
                SQL = $"select EMP_NO,SERIAL_NUMBER,KEY_PART_NO,KEY_PART_SN,KP_RELATION,GROUP_NAME,CARTON_NO,TO_CHAR (WORK_TIME, 'yyyy/mm/dd hh24:mi:ss') WORK_TIME,VERSION,PART_MODE,KP_CODE,MO_NUMBER from sfism4.R108 where serial_number = '{model.value}'";
            }
            else if (model.field == "BOM_NO")
            {
                SQL = $"select * from SFIS1.Cmodel_KEYPART_T where bom_no = '{model.value}' order by kp_relation";
            }

            builder.Append(SQL);

            string queryString = builder.ToString();

            DataTable dtCheck = DBConnect.GetData(queryString, model.database);
            if (dtCheck.Rows.Count == 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "fail" });
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { result = "ok", data = dtCheck });
            }
        }
    }
}