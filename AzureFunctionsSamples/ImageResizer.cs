using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsSamples
{
    public static class ImageResizer
    {


        /// <summary>
        /// Esta función escucha un archivo de imagen en un blob storage y la procesa para 
        /// cambiae elm tamaño de la misma: una para la web y otra para mobile
        /// </summary>
        /// <param name="myBlob">Archivo o blob de entrada</param>
        /// <param name="mobile">Archivo o blob de salida para mobile</param>
        /// <param name="web">Archivo o blob de salida para web</param>
        /// <param name="collector">Objeto para escribir en una Queue</param>
        /// <param name="name">Nombre del archivo incluyendo la extensión</param>
        /// <param name="log">Objeto para escriboir en un log o consola</param>
        /// <returns></returns>
        [FunctionName("ImageResizer")]
        public static async Task Run([BlobTrigger("resize/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, 
            [Blob("resized/mobile-{name}", FileAccess.Write, Connection ="AzureWebJobsStorage")] Stream mobile,
            [Blob("resized/web-{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream web,
            [Queue("resized-queue", Connection ="AzureWebJobsStorage")] IAsyncCollector<string> collector,
            string name, 
            ILogger log)
        {
            var processed = await ResizeToSmallImageAsync(myBlob);
            await processed.CopyToAsync(mobile);
            await collector.AddAsync("Imagen para mobile procesada");

            processed = await ResizeToSmallImageAsync(myBlob);
            await processed.CopyToAsync(web);
            await collector.AddAsync("Imagen para web procesada");

        }


        /// <summary>
        /// Método mock para redimensionar un imagen. En la vida real se usaría una librería
        /// </summary>
        /// <param name="toResize">Stream o archivo a redimiensionar</param>
        /// <returns>Retorna un Stream (o archivo) redimensionado</returns>
        private static Task<Stream> ResizeToSmallImageAsync(Stream toResize)
        {
            toResize.Position = 0;
            return Task.FromResult(toResize);
        }
    }
}
