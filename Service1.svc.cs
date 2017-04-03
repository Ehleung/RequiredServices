
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
        public string getReviews(string vendorName)
        {
            string url = @"https://api.foursquare.com/v2/venues/search?near=arizona" + "&query=" + vendorName +
                "&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K" +
                "&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=" + DateTime.Now.ToString("yyyyMMdd");
            /*https://api.foursquare.com//v2//venues//search?near=arizona&query=campground&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=20170329   */

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String json = reader.ReadToEnd();
            
            venues venueList = ParseJsonObject<venues>(json);

            setReplies(venueList);

            int count = 0;
            bool found = false;
            string vendorId = "blank";
            while (count < retainer.replies.Count && !found)
            {
                if (retainer.replies[count].name == vendorName)
                {
                    found = true;
                    vendorId = retainer.replies[count].id;
                }
                count++;
            }
            return vendorId;
            
            url = @"https://api.foursquare.com/v2/venues/" + vendorId + "/tips";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = request.GetResponse();
            responseStream = response.GetResponseStream();
            reader = new StreamReader(responseStream);
            json = reader.ReadToEnd();

            return json;
            //Result r = new Result();
            //r.response = vendorName + " has a rating of x.x stars on Foursquare.";
            //return r;
        }

        public static T ParseJsonObject<T>(string json) where T : class, new()
        {
            JObject jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        public void setReplies(venues venueList)
        {
            retainer.replies = new List<reply>();
            foreach (Venue venue in venueList.Response.Venues)
            {
                reply newReply = new reply();
                newReply.name = venue.Name;
                newReply.location = venue.Location.Address;
                newReply.id = venue.Id;
                retainer.replies.Add(newReply);
            }
        }

        public string findNearbyVenues(string location, string venueName)
        {
            string url = @"https://api.foursquare.com/v2/venues/search?near=" + location + "&query=" + venueName +
                "&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K" +
                "&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=" + DateTime.Now.ToString("yyyyMMdd");
            /*https://api.foursquare.com//v2//venues//search?near=85048&query=campground&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=20170329   */

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String json = reader.ReadToEnd();
            venues venueList = ParseJsonObject<venues>(json);

            setReplies(venueList);

            string returnStmt = "Nearby " + venueName + " around " + location + ":";
            foreach(reply replyIter in retainer.replies)
                returnStmt += "\n" + replyIter.name + ", " + replyIter.location;
            return returnStmt;
        }
        
        [Serializable]
        public class Meta
        {

            [JsonProperty("code")]
            public int Code { get; set; }

            [JsonProperty("requestId")]
            public string RequestId { get; set; }
        }
        [Serializable]
        public class Contact
        {

            [JsonProperty("phone")]
            public string Phone { get; set; }

            [JsonProperty("formattedPhone")]
            public string FormattedPhone { get; set; }
        }
        [Serializable]
        public class LabeledLatLng
        {

            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }
        [Serializable]
        public class Location
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }
            [JsonProperty("lng")]
            public double Lng { get; set; }
            [JsonProperty("labeledLatLngs")]
            public LabeledLatLng[] LabeledLatLngs { get; set; }
            [JsonProperty("postalCode")]
            public string PostalCode { get; set; }
            [JsonProperty("cc")]
            public string Cc { get; set; }
            [JsonProperty("city")]
            public string City { get; set; }
            [JsonProperty("state")]
            public string State { get; set; }
            [JsonProperty("country")]
            public string Country { get; set; }
            [JsonProperty("formattedAddress")]
            public string[] FormattedAddress { get; set; }
            [JsonProperty("address")]
            public string Address { get; set; }
            [JsonProperty("crossStreet")]
            public string CrossStreet { get; set; }
        }
        [Serializable]
        public class Icon
        {

            [JsonProperty("prefix")]
            public string Prefix { get; set; }

            [JsonProperty("suffix")]
            public string Suffix { get; set; }
        }
        [Serializable]
        public class Category
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("pluralName")]
            public string PluralName { get; set; }

            [JsonProperty("shortName")]
            public string ShortName { get; set; }

            [JsonProperty("icon")]
            public Icon Icon { get; set; }

            [JsonProperty("primary")]
            public bool Primary { get; set; }
        }
        [Serializable]
        public class Stats
        {

            [JsonProperty("checkinsCount")]
            public int CheckinsCount { get; set; }

            [JsonProperty("usersCount")]
            public int UsersCount { get; set; }

            [JsonProperty("tipCount")]
            public int TipCount { get; set; }
        }
        [Serializable]
        public class BeenHere
        {

            [JsonProperty("lastCheckinExpiredAt")]
            public int LastCheckinExpiredAt { get; set; }
        }
        [Serializable]
        public class Specials
        {

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("items")]
            public object[] Items { get; set; }
        }
        [Serializable]
        public class HereNow
        {

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("groups")]
            public object[] Groups { get; set; }
        }
        [Serializable]
        public class Venue
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("contact")]
            public Contact Contact { get; set; }
            [JsonProperty("location")]
            public Location Location { get; set; }
            [JsonProperty("categories")]
            public Category[] Categories { get; set; }
            [JsonProperty("verified")]
            public bool Verified { get; set; }
            [JsonProperty("stats")]
            public Stats Stats { get; set; }
            [JsonProperty("beenHere")]
            public BeenHere BeenHere { get; set; }
            [JsonProperty("specials")]
            public Specials Specials { get; set; }
            [JsonProperty("hereNow")]
            public HereNow HereNow { get; set; }
            [JsonProperty("referralId")]
            public string ReferralId { get; set; }
            [JsonProperty("venueChains")]
            public object[] VenueChains { get; set; }
            [JsonProperty("hasPerk")]
            public bool HasPerk { get; set; }
        }
        [Serializable]
        public class Center
        {

            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }
        [Serializable]
        public class Ne
        {

            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }
        [Serializable]
        public class Sw
        {

            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lng")]
            public double Lng { get; set; }
        }
        [Serializable]
        public class Bounds
        {

            [JsonProperty("ne")]
            public Ne Ne { get; set; }

            [JsonProperty("sw")]
            public Sw Sw { get; set; }
        }
        [Serializable]
        public class Geometry
        {

            [JsonProperty("center")]
            public Center Center { get; set; }

            [JsonProperty("bounds")]
            public Bounds Bounds { get; set; }
        }
        [Serializable]
        public class Feature
        {

            [JsonProperty("cc")]
            public string Cc { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("displayName")]
            public string DisplayName { get; set; }

            [JsonProperty("matchedName")]
            public string MatchedName { get; set; }

            [JsonProperty("highlightedName")]
            public string HighlightedName { get; set; }

            [JsonProperty("woeType")]
            public int WoeType { get; set; }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("longId")]
            public string LongId { get; set; }

            [JsonProperty("geometry")]
            public Geometry Geometry { get; set; }
        }
        [Serializable]
        public class Geocode
        {

            [JsonProperty("what")]
            public string What { get; set; }

            [JsonProperty("where")]
            public string Where { get; set; }

            [JsonProperty("feature")]
            public Feature Feature { get; set; }

            [JsonProperty("parents")]
            public object[] Parents { get; set; }
        }

        [Serializable]
        public class Response
        {

            [JsonProperty("venues")]
            public Venue[] Venues { get; set; }

            [JsonProperty("geocode")]
            public Geocode Geocode { get; set; }
        }

        [Serializable]
        public class venues
        {
            [JsonProperty("meta")]
            public Meta Meta { get; set; }

            [JsonProperty("response")]
            public Response Response { get; set; }
        }
    }
}