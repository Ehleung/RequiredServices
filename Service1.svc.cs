using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Net;
using System.IO;

namespace RequiredServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string getReview(string vendorName)
        {



            return vendorName + " has a rating of x.x stars on Yelp.";
        }

<<<<<<< HEAD
        public async Task<Yelp.Api.Models.SearchResponse> findNearestVenue(string location, string venueName)
=======
        public string findNearestVenue(string location, string venueName)
>>>>>>> parent of 1ccf77a... Updated Venue Svc
        {
            string url = @"https://api.foursquare.com/v2/venues/search?near=" + location + "&query=" + venueName;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

<<<<<<< HEAD
            var client = new Yelp.Api.Client("WWNzN00w_IDH-KeaU0Evkg", "KUu2mlUbI3q7GWHmC9Uwl7arfNTBMtj7DN9B2tU81wlTRaXRnACtFuCrkVBNwTVy");
            Yelp.Api.Models.SearchResponse results = await client.SearchBusinessesAllAsync(request);
            Console.WriteLine(results);
            //return "The closest " + venueName + " in " + location + " is ";
            return results;
=======
            StreamReader reader = new StreamReader(responseStream);
            String json = reader.ReadToEnd();

            return "The closest venue to " + location + " is ";
>>>>>>> parent of 1ccf77a... Updated Venue Svc
        }
    }
}
