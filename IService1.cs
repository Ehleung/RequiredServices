using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RequiredServices
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "getReviews/{vendor}", ResponseFormat = WebMessageFormat.Json)]
        string getReviews(string vendor);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findNearbyVenues/{location}/{venuename}", ResponseFormat = WebMessageFormat.Json)]
        string findNearbyVenues(string location, string venueName);

        [OperationContract]
        [WebInvoke(Method = "POST")]
        void setReplies(Service1.venues venueList);
    }

    [DataContract]
    public static class retainer
    {
        public static List<reply> replies;
    }

    [DataContract]
    public class reply
    {
        public string name { get; set; }
        public string location { get; set; }
        public string id { get; set; }
    }
}
