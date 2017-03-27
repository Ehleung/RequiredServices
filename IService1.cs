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
        [WebInvoke(Method = "GET", UriTemplate = "getReview/{vendor}", ResponseFormat = WebMessageFormat.Json)]
        Result getReview(string vendor);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findNearestVenue/{location}/{venuename}", ResponseFormat = WebMessageFormat.Json)]
        Result findNearestVenue(string location, string venueName);
    }

    [DataContract]
    public class Result
    {
        public string response { get; set; }
    }
}
