
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

        public void setReviews(reviews reviewList)
        {
            retainer.reviews = new List<review>();
            foreach(Item tip in reviewList.Response.Tips.Items)
            {
                review newReview = new review();
                newReview.tip = tip.Text;
                retainer.reviews.Add(newReview);
            }
        }

        public string getReviews(string vendorName)
        {
            if (Char.IsLower(vendorName[0]))
                Char.ToUpper(vendorName[0]);

            string url = @"https://api.foursquare.com/v2/venues/search?near=arizona" + "&query=" + vendorName +
                "&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K" +
                "&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=" + DateTime.Now.ToString("yyyyMMdd");
            /*https://api.foursquare.com//v2//venues//search?near=arizona&query=&client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=20170329   */

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            String json = reader.ReadToEnd();

            venues venueList = ParseJsonObject<venues>(json);

            setReplies(venueList);

            int count = 0;
            bool found = false;
            string vendorId = null;
            while (count < retainer.replies.Count && !found)
            {
                if (retainer.replies[count].name.Contains(vendorName))
                {
                    found = true;
                    vendorId = retainer.replies[count].id;
                }
                count++;
            }

            if (vendorId == null)
            {
                return "No matching venue found. Please ensure proper punctuation (capitalization, spelling) and try again.";
            }
            else
            {
                url = @"https://api.foursquare.com/v2/venues/" + vendorId + "/tips?" +
                    "client_id=Y5R4TGDCYYCLUEIXQL25EDTATVYQW5RA34NFZTY4NVNL2Z1K" +
                    "&client_secret=HOWS12EJTFIAKE3WBEJC5ZA5DF3F31DRF1RYC1GINDMU53PY&v=" + DateTime.Now.ToString("yyyyMMdd");

                request = (HttpWebRequest)WebRequest.Create(url);
                response = request.GetResponse();
                responseStream = response.GetResponseStream();
                reader = new StreamReader(responseStream);
                json = reader.ReadToEnd();
                reviews reviewList = ParseJsonObject<reviews>(json);

                setReviews(reviewList);

                string returnStmt = "Anonymous reviews for " + retainer.replies[count--].id + ": ";
                foreach (review rev in retainer.reviews)
                    returnStmt += "\n" + rev.tip;
                
                return returnStmt;
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
            foreach (reply replyIter in retainer.replies)
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
            // only for reviews
            [JsonProperty("tips")]
            public Tips Tips { get; set; }

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


        public class Source
        {

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Photo
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("createdAt")]
            public int CreatedAt { get; set; }

            [JsonProperty("source")]
            public Source Source { get; set; }

            [JsonProperty("prefix")]
            public string Prefix { get; set; }

            [JsonProperty("suffix")]
            public string Suffix { get; set; }

            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("visibility")]
            public string Visibility { get; set; }
        }

        public class Photo2
        {

            [JsonProperty("prefix")]
            public string Prefix { get; set; }

            [JsonProperty("suffix")]
            public string Suffix { get; set; }

            [JsonProperty("default")]
            public bool? Default { get; set; }
        }

        public class Item2
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("gender")]
            public string Gender { get; set; }

            [JsonProperty("photo")]
            public Photo2 Photo { get; set; }
        }

        public class Group
        {

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("items")]
            public Item2[] Items { get; set; }
        }

        public class Likes
        {

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("groups")]
            public Group[] Groups { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }
        }

        public class Todo
        {

            [JsonProperty("count")]
            public int Count { get; set; }
        }

        public class Photo3
        {

            [JsonProperty("prefix")]
            public string Prefix { get; set; }

            [JsonProperty("suffix")]
            public string Suffix { get; set; }

            [JsonProperty("default")]
            public bool? Default { get; set; }
        }

        public class User
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("gender")]
            public string Gender { get; set; }

            [JsonProperty("photo")]
            public Photo3 Photo { get; set; }

            [JsonProperty("lastName")]
            public string LastName { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class Item
        {

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("createdAt")]
            public int CreatedAt { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("canonicalUrl")]
            public string CanonicalUrl { get; set; }

            [JsonProperty("photo")]
            public Photo Photo { get; set; }

            [JsonProperty("photourl")]
            public string Photourl { get; set; }

            [JsonProperty("likes")]
            public Likes Likes { get; set; }

            [JsonProperty("logView")]
            public bool LogView { get; set; }

            [JsonProperty("agreeCount")]
            public int AgreeCount { get; set; }

            [JsonProperty("disagreeCount")]
            public int DisagreeCount { get; set; }

            [JsonProperty("lastVoteText")]
            public string LastVoteText { get; set; }

            [JsonProperty("lastUpvoteTimestamp")]
            public int LastUpvoteTimestamp { get; set; }

            [JsonProperty("todo")]
            public Todo Todo { get; set; }

            [JsonProperty("user")]
            public User User { get; set; }

            [JsonProperty("authorInteractionType")]
            public string AuthorInteractionType { get; set; }

            [JsonProperty("url")]
            public string Url { get; set; }
        }

        public class Tips
        {

            [JsonProperty("count")]
            public int Count { get; set; }

            [JsonProperty("items")]
            public Item[] Items { get; set; }
        }

        public class reviews
        {
            [JsonProperty("meta")]
            public Meta Meta { get; set; }

            [JsonProperty("response")]
            public Response Response { get; set; }
        }
    }
}