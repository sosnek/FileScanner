using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FileScan
{
    class APIHelper
    {
        /// <summary>
        /// static HttpClient to be used throughout the application for hitting the back-end API
        /// </summary>
        public static HttpClient ApiClient { get; set; }

        /// <summary>
        /// Instantiates the ApiClient (HttpClient object). Sets the BaseAddress (URL of the API) and 
        /// the default headers. Adds the auth token. Everything needed to hit the API except the 
        /// specific endpoint to hit, and the payload.
        /// </summary>
        static APIHelper()
        {
            ApiClient = new HttpClient();
            ApiClient.BaseAddress = new Uri("https://www.virustotal.com"); // API url
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
