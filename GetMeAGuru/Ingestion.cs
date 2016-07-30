
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace GetMeAGuru
{
     
    public class Ingestion
    {

        private static readonly string EndpointUri = ConfigurationManager.AppSettings["EndPointUrl"];
        private static readonly string PrimaryKey = ConfigurationManager.AppSettings["DBAuthorizationKey"];
        private static readonly string databaseId = ConfigurationManager.AppSettings["DatabaseId"];
        private static readonly string collectionId = ConfigurationManager.AppSettings["CollectionId"];

        private DocumentClient client;

        public static void Main(string[] args)
        {
            try
            {
                Ingestion i = new Ingestion();
                i.PushDocument().Wait();
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
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }
        }
        
        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }


        private async Task PushDocument()
        {
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            await this.CreateDatabaseIfNotExists("GuruDB");

            // ADD THIS PART TO YOUR CODE
            await this.CreateDocumentCollectionIfNotExists("GuruDB", "gurus");


            // ADD THIS PART TO YOUR CODE
            Guru guru = new Guru
            {
                Id = "41",
                alias = "stmich",
                topic = new expertises[]
                    {
                new expertises { title = "Windows Development" },
                new expertises { title = "Office 365" },
                new expertises { title = "Azure" },
                new expertises { title = "DevOps" },
                new expertises { title = "Game Dev" },
                new expertises { title = "Web Dev" },
                new expertises { title = "Machine Learning" },
                new expertises { title = "IOT" },
                new expertises { title = "Media" },
                new expertises { title = "High Scale Data"},
                    },
                 session = new engagements[]
                    {
                new engagements
                    {
                        title = "Introduction to GuruBot",
                        description = "Anders explains infull detail why the GuruBot is awesome.",
                        location = "Redmond",
                        company = "Microsoft",
                        date = "2016-06-27",
                        domain = new tech[]
                        {
                                new tech { domain = "Azure" },
                                new tech { domain = "Web Dev" },
                                new tech { domain = "High Scale Data" }
                        },
                        url = new resources[]
                        {
                                new resources { url = "https://github.com/Lybecker/GetMeAGuruBot" },
                                new resources { url = "https://gurubot.azurewebsites.net" }
                        }
                    }
                },
            };

            await this.CreateGuruDocumentIfNotExists("GuruDB", "gurus", guru);
            
        }


        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=GuruDB does not exist
            try
            {
                await this.client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", databaseName);
            }
            catch (DocumentClientException de)
            {
                // If the database does not exist, create a new database
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDatabaseAsync(new Database { Id = databaseName });
                    this.WriteToConsoleAndPromptToContinue("Created {0}", databaseName);
                }
                else
                {
                    throw;
                }
            }
        }


        private async Task CreateDocumentCollectionIfNotExists(string databaseName, string collectionName)
        {
            try
            {
                await this.client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName));
                this.WriteToConsoleAndPromptToContinue("Found {0}", collectionName);
            }
            catch (DocumentClientException de)
            {
                // If the document collection does not exist, create a new collection
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    DocumentCollection collectionInfo = new DocumentCollection();
                    collectionInfo.Id = collectionName;

                    // Configure collections for maximum query flexibility including string range queries.
                    collectionInfo.IndexingPolicy = new IndexingPolicy(new RangeIndex(DataType.String) { Precision = -1 });

                    // Here we create a collection with 400 RU/s.
                    await this.client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseName),
                        collectionInfo,
                        new RequestOptions { OfferThroughput = 400 });

                    this.WriteToConsoleAndPromptToContinue("Created {0}", collectionName);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task CreateGuruDocumentIfNotExists(string databaseName, string collectionName, Guru guru)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, guru.Id));
                this.WriteToConsoleAndPromptToContinue("Found {0}", guru.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), guru);
                    this.WriteToConsoleAndPromptToContinue("Created Guru {0}", guru.Id);
                }
                else
                {
                    throw;
                }
            }
        }

    }
}