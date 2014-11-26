using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class JSONResponse
    {
        public int ID { get; set; }

        public string responseType { get; set; }

        public string response { get; set; }


        public static string ConvertToString(JSONResponse jsonResponse)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return ResponseTypes.BadResponse;
            }
        }

        public static JSONResponse ConvertToJson(string responseToConvert)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONResponse>(responseToConvert);
            }
            catch(Newtonsoft.Json.JsonReaderException ex)
            {
                JSONResponse badRsp = new JSONResponse();
                badRsp.ID = -1;
                badRsp.responseType = ResponseTypes.BadResponse;
                badRsp.response = ex.Message;

                return badRsp;
            }
        }
    }
}
