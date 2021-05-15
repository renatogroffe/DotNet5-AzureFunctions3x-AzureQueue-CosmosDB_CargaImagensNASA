using System;
using System.Linq;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;

namespace FunctionAppImagensNASA.Data
{
    public static class ImagensNASARepository
    {
        private static readonly string _BaseNASA;
        private static readonly string _Container;

        static ImagensNASARepository()
        {
            _BaseNASA = Environment.GetEnvironmentVariable("BaseNASA");
            _Container = Environment.GetEnvironmentVariable("ContainerImagensNASA");

            using var client = GetCosmosClient();

            Database database =
                client.CreateDatabaseIfNotExistsAsync(_BaseNASA).Result;
            database.CreateContainerIfNotExistsAsync(
                _Container, "/nomeArquivoImagem").Wait();
        }

        private static CosmosClient GetCosmosClient()
        {
            return new CosmosClientBuilder(
                Environment.GetEnvironmentVariable("CosmosDBConnectionString"))
                    .WithSerializerOptions(
                        new CosmosSerializationOptions()
                        {
                            IgnoreNullValues = true,
                            PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                        })
                    .Build();
        }

        public static void Save(CargaImagemNASADocument document)
        {
            using var client = GetCosmosClient();
            client.GetContainer(_BaseNASA, _Container)
                .CreateItemAsync(document, new PartitionKey(document.NomeArquivoImagem))
                .Wait();
        }

        public static CargaImagemNASADocument[] GetAll()
        {
            using var client = GetCosmosClient();
            return client.GetContainer(_BaseNASA, _Container)
                .GetItemLinqQueryable<CargaImagemNASADocument>(
                    allowSynchronousQueryExecution: true)
                .OrderByDescending(d => d.id).ToArray();
        }
    }
}