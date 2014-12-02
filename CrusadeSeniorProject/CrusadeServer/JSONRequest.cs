using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrusadeServer
{
    public class JSONRequest
    {
        public string RequestIP { get; set; }

        public int RequestPort { get; set; }

        public byte RequestType { get; set; }

        public string Request { get; set; }


        public static string ConvertToString(JSONRequest jsonRequest)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(jsonRequest);
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return ConvertToString(CreateBadRequest());
            }
        }

        public static JSONRequest ConvertToJson(string requestToConvert)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JSONRequest>(requestToConvert.Replace("%", ""));
            }
            catch(Newtonsoft.Json.JsonReaderException)
            {
                return CreateBadRequest();
            }
        }


        private static JSONRequest CreateBadRequest()
        {
            JSONRequest badRequest = new JSONRequest();
            badRequest.RequestIP = string.Empty;
            badRequest.RequestPort = -1;
            badRequest.RequestType = RequestTypes.BadRequest;
            badRequest.Request = "Bad REQ";

            return badRequest;
        }

    }
}
