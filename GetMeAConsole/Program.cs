using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Newtonsoft.Json;

namespace GetMeAConsole
{
    class Program
    {
        private const string EndpointUri = "https://gurubotfb.documents.azure.com:443/";
        private const string PrimaryKey = "44INVS89aLcOC0LKlBjPRpSFBrPTTyuEltne8faLuAEEZIDfH5UnD93xBZEzUQirg9LevPc3QBKcdVEKgqsMBg==";

        private DocumentClient client;

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("gaan   !");
                Program p = new Program();
                p.GetStartedDemo().Wait();
                Console.WriteLine("gedaan :)");
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

        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUri), PrimaryKey);

            await this.CreateDatabaseIfNotExists("TestDB");

            // ADD THIS PART TO YOUR CODE
            await this.CreateDocumentCollectionIfNotExists("TestDB", "gurus");


            // ADD THIS PART TO YOUR CODE
            Family andersenFamily = new Family
            {
                Id = "Andersen.1",
                LastName = "Andersen",
                Parents = new Parent[]
                    {
                new Parent { FirstName = "Thomas" },
                new Parent { FirstName = "Mary Kay" }
                    },
                Children = new Child[]
                    {
                new Child
                {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new Pet[]
                        {
                                new Pet { GivenName = "Fluffy" }
                        }
                }
                    },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = true
            };

            await this.CreateFamilyDocumentIfNotExists("TestDB", "gurus", andersenFamily);

            Family wakefieldFamily = new Family
            {
                Id = "Wakefield.7",
                LastName = "Wakefield",
                Parents = new Parent[]
                    {
                new Parent { FamilyName = "Wakefield", FirstName = "Robin" },
                new Parent { FamilyName = "Miller", FirstName = "Ben" }
                    },
                Children = new Child[]
                    {
                new Child
                {
                        FamilyName = "Merriam",
                        FirstName = "Jesse",
                        Gender = "female",
                        Grade = 8,
                        Pets = new Pet[]
                        {
                                new Pet { GivenName = "Goofy" },
                                new Pet { GivenName = "Shadow" }
                        }
                },
                new Child
                {
                        FamilyName = "Miller",
                        FirstName = "Lisa",
                        Gender = "female",
                        Grade = 1
                }
                    },
                Address = new Address { State = "NY", County = "Manhattan", City = "NY" },
                IsRegistered = false
            };

            await this.CreateFamilyDocumentIfNotExists("TestDB", "gurus", wakefieldFamily);
        }

        private void WriteToConsoleAndPromptToContinue(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            Console.WriteLine("Press any key to continue ...");
            Console.ReadKey();
        }

        // ADD THIS PART TO YOUR CODE
        public class Family
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }
            public string LastName { get; set; }
            public Parent[] Parents { get; set; }
            public Child[] Children { get; set; }
            public Address Address { get; set; }
            public bool IsRegistered { get; set; }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class Parent
        {
            public string FamilyName { get; set; }
            public string FirstName { get; set; }
        }

        public class Child
        {
            public string FamilyName { get; set; }
            public string FirstName { get; set; }
            public string Gender { get; set; }
            public int Grade { get; set; }
            public Pet[] Pets { get; set; }
        }

        public class Pet
        {
            public string GivenName { get; set; }
        }

        public class Address
        {
            public string State { get; set; }
            public string County { get; set; }
            public string City { get; set; }
        }

        private async Task CreateDatabaseIfNotExists(string databaseName)
        {
            // Check to verify a database with the id=FamilyDB does not exist
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

        // ADD THIS PART TO YOUR CODE
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
        private async Task CreateFamilyDocumentIfNotExists(string databaseName, string collectionName, Family family)
        {
            try
            {
                await this.client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, family.Id));
                this.WriteToConsoleAndPromptToContinue("Found {0}", family.Id);
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await this.client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), family);
                    this.WriteToConsoleAndPromptToContinue("Created Family {0}", family.Id);
                }
                else
                {
                    throw;
                }
            }
        }

    }
