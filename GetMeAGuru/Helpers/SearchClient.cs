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
        string readwriteApiKey = "1A00958EC2CF8C40EE789F0E69520E73";
        string index = "gurus";

        //tech, location/area, level

        public IList<SearchGuru> Search(string searchText)
        {
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(readonlyApiKey));

            var indexClient = serviceClient.Indexes.GetClient(index);

            var sp = new SearchParameters();

            //if (!String.IsNullOrEmpty(tech))
            //{
            //    sp.Filter = "engagments/tech/any(t: t eq '" + tech + "')"; // and location eq '" + location+ "'"; // and level eg '{level}'";
            //}

            DocumentSearchResult<SearchGuru> response = indexClient.Documents.Search<SearchGuru>(searchText, sp);
            //foreach (SearchResult<Engagement> result in response.Results)
            //{
            //    Console.WriteLine(result.Document);
            //}

            return response.Results.Select(x => x.Document).ToList();
            //return response.Results.Select(x => x.Document).Select(x => new Guru() { alias = x.Alias }).ToList();

        }

        public void Add(SearchGuru item)
        {
            SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(readwriteApiKey));

            var indexClient = serviceClient.Indexes.GetClient(index);

            //indexClient.Documents.Index(IndexBatch.Upload(new SearchGuru[] {
            //    new SearchGuru()
            //    {
            //        Alias = item.alias,
            //        Tech = (from s in item.session
            //               from t in s.domain
            //               select t.domain).ToList()
            //    }
            //}));

            indexClient.Documents.Index(IndexBatch.Upload(new SearchGuru[] { item }));
        }
    }

    public class SearchGuru
    {
        public SearchGuru()
        {
            techs= new List<string>();
        }
        public string alias { get; set; }
        public IList<string> techs { get; set; }
    }
}