using AddressFinder.Model;
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
    public class GooglePlacesApiBroker
    {
        public static List<LocationInfo> FindNearby(string latLong, string locationType = null, string nextPageToken = null)
        {
            List<LocationInfo> responseObj = null;
            try
            {
                string googleGeoCodeEndPoint = ConfigurationManager.AppSettings["GooglePlacesApiEndPoint"];
                var requestUri = string.Format("{0}json?location={1}&radius={2}&key={3}", googleGeoCodeEndPoint, Uri.EscapeDataString(latLong), ConfigurationManager.AppSettings["GooglePlacesRadius"], ConfigurationManager.AppSettings["GooglePlacesApiKey"]);
                if (string.IsNullOrWhiteSpace(locationType) == false)
                    requestUri = string.Format("{0}&type={1}", requestUri, locationType);

                if (string.IsNullOrWhiteSpace(nextPageToken) == false)
                    requestUri = string.Format("{0}&pagetoken={1}", requestUri, nextPageToken);

                using (WebClient wc = new WebClient())
                {
                    wc.Encoding = System.Text.Encoding.UTF8;
                    wc.Headers["User-Agent"] = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET4.0C; .NET4.0E)";
                    string responseString = wc.DownloadString(new Uri(requestUri));
                    var responseJson = JObject.Parse(responseString);
                    if (responseJson != null)
                    {
                        if (responseJson["results"] != null)
                        {
                            if (responseObj == null)
                                responseObj = new List<LocationInfo>();
                            foreach (var item in ((JArray)responseJson["results"]))
                            {
                                var obj = new LocationInfo((JObject)item);
                                obj.Distance=GeoCodeCalc.CalcDistance(latLong, obj.Latitute + "," + obj.Longitude, GeoCodeCalcMeasurement.Kilometers);
                                responseObj.Add(obj);


                            }
                        }

                        if (responseJson["next_page_token"] != null)
                        {
                            nextPageToken = responseJson["next_page_token"].ToString();
                            var nextResults = FindNearby(latLong, locationType, nextPageToken);
                            if (nextResults != null && nextResults.Count > 0)
                            {
                                if (responseObj == null)
                                    responseObj = new List<LocationInfo>();
                                responseObj.AddRange(nextResults);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return responseObj;
        }

    }
    public static class GeoCodeCalc
    {
        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;

        public static double ToRadian(double val) { return val * (Math.PI / 180); }
        public static double DiffRadian(double val1, double val2) { return ToRadian(val2) - ToRadian(val1); }

        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2)
        {
            return CalcDistance(lat1, lng1, lat2, lng2, GeoCodeCalcMeasurement.Miles);
        }

        public static double CalcDistance(double lat1, double lng1, double lat2, double lng2, GeoCodeCalcMeasurement m)
        {
            double radius = GeoCodeCalc.EarthRadiusInMiles;

            if (m == GeoCodeCalcMeasurement.Kilometers) { radius = GeoCodeCalc.EarthRadiusInKilometers; }
            return radius * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }
        public static double CalcDistance(string latLng1, string latLng2, GeoCodeCalcMeasurement m)
        {
            try
            {
                double lat1, lng1, lat2, lng2;
                string[] latlngArray = latLng1.Split(',');
                lat1 = Convert.ToDouble(latlngArray[0]);
                lng1 = Convert.ToDouble(latlngArray[1]);

                latlngArray = latLng2.Split(',');
                lat2 = Convert.ToDouble(latlngArray[0]);
                lng2 = Convert.ToDouble(latlngArray[1]);

                return GeoCodeCalc.CalcDistance(lat1, lng1, lat2, lng2, m);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }

    public enum GeoCodeCalcMeasurement : int
    {
        Miles = 0,
        Kilometers = 1
    }
}
