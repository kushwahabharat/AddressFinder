using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressFinder.Model
{
    public class LocationInfo
    {
        public string Name { get; set; }
        public string Latitute { get; set; }
        public string Longitude { get; set; }
        public string Types { get; set; }
        public string Address { get; set; }
      //  public string ZipCode { get; set; }
        public string Accuracy { get; set; }
        public double Distance { get; set; }


        public LocationInfo(string name = null, string lat = null, string lng = null, string types = null, string address = null)
        {
            if (string.IsNullOrWhiteSpace(name) == false)
                Name = name;
            if (string.IsNullOrWhiteSpace(lat) == false)
                Latitute = lat;
            if (string.IsNullOrWhiteSpace(lng) == false)
                Longitude = lng;
            if (string.IsNullOrWhiteSpace(types) == false)
                Types = types;
            if (string.IsNullOrWhiteSpace(address) == false)
                Address = address;
        }
        public LocationInfo(JObject item, bool isGeoCodeItem = false)
        {
            if (item == null)
                return;
            if (isGeoCodeItem)
                SetLocationInfoFromGeoCodeObj(item);
            else
                SetLocationInfoFromPlacesObj(item);
        }

        private void SetLocationInfoFromGeoCodeObj(JObject item)
        {
            if (item["formatted_address"] != null)
                Name = Address = item["formatted_address"].ToString();

            if (item["geometry"] != null)
            {
                BindGeometryInformation((JObject)item["geometry"]);
            }

            if (item["types"] != null)
                Types = item["types"].ToString();

        }

        public void BindGeometryInformation(JObject geometry)
        {
            if (geometry["location"] != null)
            {
                var location = geometry["location"];
                if (location["lat"] != null)
                    Latitute = location["lat"].ToString();

                if (location["lng"] != null)
                    Longitude = location["lng"].ToString();

                if (geometry["location_type"] != null)
                    Accuracy = geometry["location_type"].ToString();
            }
        }
        public void SetLocationInfoFromPlacesObj(JObject item)
        {
            if (item["name"] != null)
                Name = item["name"].ToString();
            if (item["geometry"] != null)
            {
                BindGeometryInformation((JObject)item["geometry"]);
            }
            if (item["types"] != null)
                Types = item["types"].ToString();
        }
    }
}
