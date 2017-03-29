using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Web;


namespace RequiredServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Result getReview(string vendorName)
        {
            Result r = new Result();
            r.response = vendorName + " has a rating of x.x stars on Yelp.";
            return r;
        }

        public string findNearbyVenues(string location, string venueName)
        {
            string url = @"https://api.foursquare.com/v2/venues/search?near=" + location + "&query=" + venueName +
                "&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K" +
                "&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=" + DateTime.Now.ToString("yyyyMMdd");

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String json = reader.ReadToEnd();

            JavaScriptSerializer ser = new JavaScriptSerializer();

            venueList array = ser.Deserialize<venueList>(json);
            return ser.Serialize(array);

            //Result r = new Result();
            //r.response = "The closest venue to " + venueName + " is "  + " meters.";
            //return r.response;
        }

        [Serializable]
        public class locations
        {

        }

        [Serializable]
        public class venue
        {
            public string name { get; set; }
            public string location { get; set; }
            //public int distance { get; set; }
        }

        [Serializable]
        public class venueList
        {
            public List<venue> list { get; set; }
        }

        [Serializable]
        public class responses
        {
            public venueList venues { get; set; }
        }
    }
}
