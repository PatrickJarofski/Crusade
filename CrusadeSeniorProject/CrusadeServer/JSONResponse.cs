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
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonResponse);
        }

        public static JSONResponse ConvertToJson(string responseToConvert)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONResponse>(responseToConvert);
        }
    }
}
