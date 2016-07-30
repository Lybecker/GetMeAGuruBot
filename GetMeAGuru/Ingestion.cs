
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace GetMeAGuru
{     
    public class Ingestion
    {
        private static readonly string endpointUrl = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string authorizationKey = ConfigurationManager.AppSettings["DBAuthorizationKey"];
        private static readonly string databaseId = ConfigurationManager.AppSettings["DatabaseId"]; 
        private static readonly string collectionId = ConfigurationManager.AppSettings["CollectionId"];

        private static DocumentClient client;

        public static void Main(string[] args)
        {
        }


        private static async Task pushDocument(Engagements engagementObj) { 
            try
            {
                var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

                using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey)) {
                    Init();

                    Document created = await client.CreateDocumentAsync(collectionLink, engagementObj);
                    Console.WriteLine(created);
                }
            }
        }

        private static void Init()
        {
            GetOrCreateDatabaseAsync(databaseId).Wait();
            GetOrCreateCollectionAsync(databaseId, collectionId).Wait(); 
        }
        
    }
}