using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;

namespace GetMeAGuru
{
    public class SearchClient
    {
        string searchServiceName = "gurubot";
        string readonlyApiKey = "DAEC3C62F7F0D2202FA439D3EDDFD8E6";

        //tech, location/area, level

        public void Search(string searchText, string tech, string location, string level)
        {
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(readonlyApiKey));

            var indexClient = serviceClient.Indexes.GetClient("engagements");

            var sp = new SearchParameters();

            //if (!String.IsNullOrEmpty(filter))
            //{
            //    sp.Filter = filter;
            //}

            DocumentSearchResult<Engagements> response = indexClient.Documents.Search<Engagements>(searchText, sp);
            foreach (SearchResult<Engagements> result in response.Results)
            {
                Console.WriteLine(result.Document);
            }
        }
    }
}