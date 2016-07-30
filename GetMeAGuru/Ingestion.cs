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


        private static async Task pushDocument(Engagement engagementObj)
        {
            var collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

            using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey))
            {
                Init();

                Document created = await client.CreateDocumentAsync(collectionLink, engagementObj);
                Console.WriteLine(created);
            }

        }

        private static void Init()
        {
            //GetOrCreateDatabaseAsync(databaseId).Wait();
            //GetOrCreateCollectionAsync(databaseId, collectionId).Wait();
        }


        /*
        private static DocumentClient client;

        public static void Main(string[] args)
         { 
             try 
             { 
                 //Get a single instance of Document client and reuse this for all the samples 
                 //This is the recommended approach for DocumentClient as opposed to new'ing up new instances each time 
                 using (client = new DocumentClient(new Uri(endpointUrl), authorizationKey)) 
                 { 
                     //ensure the database & collection exist before running samples 
                     Init(); 


                     RunDocumentsDemo().Wait(); 

                     //Clean-up environment 
                     Cleanup(); 
                 } 
             } 
             catch (DocumentClientException de) 
             { 
                 Exception baseException = de.GetBaseException(); 
                 Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message); 
             } 
             catch (Exception e) 
             { 
                 Exception baseException = e.GetBaseException(); 
                 Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message); 
             } 
             finally 
             { 
                 Console.WriteLine("\nEnd of demo, press any key to exit."); 
                 Console.ReadKey(); 
             }
        }

        private static void Cleanup()
         { 
             client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId)).Wait(); 
         }

    private static void Init()
         { 
             GetOrCreateDatabaseAsync(databaseId).Wait(); 
           //  GetOrCreateCollectionAsync(databaseId, collectionId).Wait(); 
         }


        private static async Task<Database> GetOrCreateDatabaseAsync(string databaseId)
         { 
             var databaseUri = UriFactory.CreateDatabaseUri(databaseId); 

             Database database = client.CreateDatabaseQuery() 
                 .Where(db => db.Id == databaseId)
                 .ToArray()
                 .FirstOrDefault(); 


             if (database == null) 
             { 
                 database = await client.CreateDatabaseAsync(new Database { Id = databaseId}); 
             } 


             return database;
        } 


         /// <summary> 
         /// Run basic document access methods as a console app demo 
         /// </summary> 
         /// <returns></returns> 
         private static async Task RunDocumentsDemo()
         { 
             await RunBasicOperationsOnStronglyTypedObjects(); 
             await RunBasicOperationsOnDynamicObjects(); 
             await UseETags(); 
         } 

         /// <summary> 
         /// 1. Basic CRUD operations on a document 
         /// 1.1 - Create a document 
         /// 1.2 - Read a document by its Id 
         /// 1.3 - Read all documents in a Collection 
         /// 1.4 - Query for documents by a property other than Id 
         /// 1.5 - Replace a document 
         /// 1.6 - Upsert a document 
         /// 1.7 - Delete a document 
         /// </summary> 
         private static async Task RunBasicOperationsOnStronglyTypedObjects()
         { 
             await CreateDocumentsAsync(); 
             await ReadDocumentAsync(); 

             SalesOrder result = QueryDocuments(); 

             await ReplaceDocumentAsync(result); 
             await UpsertDocumentAsync(); 
             await DeleteDocumentAsync(); 
         } 


         private static async Task CreateDocumentsAsync()
         { 
             Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseName, collectionName); 

             Console.WriteLine("\n1.1 - Creating documents"); 

             // Create a SalesOrder object. This object has nested properties and various types including numbers, DateTimes and strings. 
             // This can be saved as JSON as is without converting into rows/columns. 
             SalesOrder salesOrder = GetSalesOrderSample("SalesOrder1"); 
             await client.CreateDocumentAsync(collectionLink, salesOrder); 

             // As your app evolves, let's say your object has a new schema. You can insert SalesOrderV2 objects without any  
             // changes to the database tier. 
             SalesOrder2 newSalesOrder = GetSalesOrderV2Sample("SalesOrder2"); 
             await client.CreateDocumentAsync(collectionLink, newSalesOrder); 
         } 


         private static async Task ReadDocumentAsync()
         { 
             Console.WriteLine("\n1.2 - Reading Document by Id"); 

             // Note that Reads require a partition key to be spcified. This can be skipped if your collection is not 
             // partitioned i.e. does not have a partition key definition during creation. 
             var response = await client.ReadDocumentAsync(
                 UriFactory.CreateDocumentUri(databaseName, collectionName, "SalesOrder1"),
                 new RequestOptions { PartitionKey = new PartitionKey("Account1") }); 

             // You can measure the throughput consumed by any operation by inspecting the RequestCharge property 
             Console.WriteLine("Document read by Id {0}", response.Resource); 
             Console.WriteLine("Request Units Charge for reading a Document by Id {0}", response.RequestCharge); 

             SalesOrder readOrder = (SalesOrder)(dynamic)response.Resource; 

             //****************************************************************************************************************** 
             // 1.3 - Read ALL documents in a Collection 
             // 
             // NOTE: Use MaxItemCount on FeedOptions to control how many documents come back per trip to the server 
             //       Important to handle throttles whenever you are doing operations such as this that might 
             //       result in a 429 (throttled request) 
             //****************************************************************************************************************** 
             Console.WriteLine("\n1.3 - Reading all documents in a collection"); 


             foreach (Document document in await client.ReadDocumentFeedAsync( 
                 UriFactory.CreateDocumentCollectionUri(databaseName, collectionName),  
                 new FeedOptions { MaxItemCount = 10 })) 
             { 
                 Console.WriteLine(document); 
             } 
         } */

    }
}