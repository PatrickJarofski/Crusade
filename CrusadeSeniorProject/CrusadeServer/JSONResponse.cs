using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class JSONResponse
    {
        public byte responseType { get; set; }

        public string response { get; set; }


        public static string ConvertToString(JSONResponse jsonResponse)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return ConvertToString(CreateBadResponse());
            }
        }

        public static JSONResponse ConvertToJson(string responseToConvert)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONResponse>(responseToConvert);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return CreateBadResponse();
            }
        }

        private static JSONResponse CreateBadResponse()
        {
            JSONResponse badResponse = new JSONResponse();

            badResponse.responseType = ResponseTypes.BadResponse;
            badResponse.response = "Bad RSP";

            return badResponse;
        }
    }
}
