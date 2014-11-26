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
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequest);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return RequestTypes.BadRequest;
            }
        }

        public static JSONRequest ConvertToJson(string requestToConvert)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRequest>(requestToConvert);
            }
            catch(Newtonsoft.Json.JsonReaderException ex)
            {
                JSONRequest badRequest = new JSONRequest();
                badRequest.ID = -1;
                badRequest.requestType = RequestTypes.BadRequest;
                badRequest.request = ex.Message;
                return badRequest;
            }
        }

    }
}
