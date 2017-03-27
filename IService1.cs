using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
<<<<<<< HEAD
using System.Threading.Tasks;
using Yelp;
=======
>>>>>>> parent of 1ccf77a... Updated Venue Svc

namespace RequiredServices
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string getReview(string vendor);

        [OperationContract]
<<<<<<< HEAD
        [WebInvoke(Method = "GET", UriTemplate = "/findNearestVenue/{location}/{venuename}", ResponseFormat = WebMessageFormat.Json)]
        Task<Yelp.Api.Models.SearchResponse> findNearestVenue(string location, string venueName);
=======
        string findNearestVenue(string location, string venueName);
>>>>>>> parent of 1ccf77a... Updated Venue Svc
    }
}
