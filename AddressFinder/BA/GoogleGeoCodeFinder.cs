using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AddressFinder.BA
{
    public class GoogleGeoCodeFinder
    {

        public static string FindAddressDetails(string address)
        {
            string googleGeoCodeEndPoint = ConfigurationManager.AppSettings["GoogleGeoCodeEndPoint"];
            var requestUri = string.Format("{0}json?address={1}&sensor=false&key={2}",googleGeoCodeEndPoint, Uri.EscapeDataString(address), ConfigurationManager.AppSettings["GoogleGeoCodeApiKey"]);

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                return wc.DownloadString(new Uri(requestUri));
            }
        }
    }
}
