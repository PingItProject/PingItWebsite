using Newtonsoft.Json;
using PingItWebsite.JsonModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace PingItWebsite.APIs
{
    public class GeocodingAPI
    {
        #region Variables
        string _APIkey = "[INSERT CREDENTIALS]";
        string _url = "https://maps.googleapis.com/maps/api/geocode/json?&address={0}%2C%20{1}&key={2}";
        #endregion

        #region Constructor
        public GeocodingAPI()
        {

        }
        #endregion

        #region Requests
        /// <summary>
        /// Get location coordinates using the Google API
        /// </summary>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public IList<double> GetLocationCoords(string city, string state)
        {
            IList<double> coords = new List<double>();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("X-API-Key", _APIkey);

            string url = String.Format(_url, city, state, _APIkey);
            HttpResponseMessage msg = null;
            try
            {
                msg = httpClient.GetAsync(url).Result;
            }
            catch (AggregateException)
            {
                Debug.WriteLine("API (Geocoding): Cannot get speed results.");
            }
            if (msg != null)
            {
                using (msg)
                {
                    if (msg.IsSuccessStatusCode)
                    {
                        var data = msg.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject<Geocode>(data);

                        //get guid
                        if (json != null)
                        {
                            coords.Add(json.results[0].geometry.location.lat);
                            coords.Add(json.results[0].geometry.location.lng);
                        }
                    }
                }
            }
            return coords;
        }
        #endregion
    }
}
