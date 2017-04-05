using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace TryIt
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string url = @"http://localhost:57310/Service1.svc/findNearbyVenues/" + TextBox1.Text + "/" + TextBox2.Text;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string json = reader.ReadToEnd();

            Label1.Text = json;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            string url = @"http://localhost:57310/Service1.svc/getReviews/" + TextBox3.Text;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string json = reader.ReadToEnd();

            Label2.Text = json;
        }
    }
}