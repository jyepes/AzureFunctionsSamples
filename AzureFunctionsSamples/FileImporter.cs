using System;
using System.IO;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSamples
{
    public static class FileImporter
    {

        /// <summary>
        /// Esta función escucha un archivo .txt con datos para importar a una Table Storage
        /// </summary>
        /// <param name="content">Contenido del archivo de texto</param>
        /// <param name="collector">Objeto para escribir en una Table Storage llamada "orders"</param>
        /// <param name="customerId">Id del cliente</param>
        /// <param name="docType">Tipo de documento</param>
        /// <param name="log">Objeto para escribir en un log o la consola</param>
        [FunctionName("FileImporter")]
        public static void Run([BlobTrigger("batch/{customerId}-{docType}.txt", Connection = "AzureWebJobsStorage")]string content,
            [Table("orders",Connection ="AzureWebJobsStorage")] ICollector<Order> collector,
            string customerId, 
            string docType,
            ILogger log)
        {
            var orders = content.Split("\n").Select(line => new Order
            {
                PartitionKey = customerId,
                RowKey = Guid.NewGuid().ToString(),
                CustomerId = customerId,
                DocType = docType,
                ProductName = line
            });

            foreach (var order in orders)
                collector.Add(order);
        }
    }

    public class Order
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string CustomerId { get; set; }
        public string DocType { get; set; }
        public string ProductName { get; set; }
    }
}
