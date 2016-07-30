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

        public IList<Guru> Search(string searchText)
        {
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(readonlyApiKey));

            var indexClient = serviceClient.Indexes.GetClient("gurus");

            var sp = new SearchParameters();

            //if (!String.IsNullOrEmpty(tech))
            //{
            //    sp.Filter = "engagments/tech/any(t: t eq '" + tech + "')"; // and location eq '" + location+ "'"; // and level eg '{level}'";
            //}

            DocumentSearchResult<Guru> response = indexClient.Documents.Search<Guru>(searchText, sp);
            //foreach (SearchResult<Engagement> result in response.Results)
            //{
            //    Console.WriteLine(result.Document);
            //}

            return response.Results.Select(x => x.Document).ToList();
        }
    }
}