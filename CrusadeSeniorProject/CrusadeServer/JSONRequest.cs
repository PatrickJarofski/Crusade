using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class JSONRequest
    {
        public int ID { get; set; }

        public string requestIP { get; set; }

        public int requestPort { get; set; }

        public string requestType { get; set; }

        public string request { get; set; }


        public static string ConvertToString(JSONRequest jsonRequest)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequest);
        }

        public static JSONRequest ConvertToJson(string requestToConvert)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRequest>(requestToConvert);
        }

    }
}
