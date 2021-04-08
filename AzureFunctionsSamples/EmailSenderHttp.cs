using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;

namespace AzureFunctionsSamples
{
    public static class EmailSenderHttp
    {

        /// <summary>
        /// Función que se dispara con una petición Http tipo POST y envía un email con lo datos recibidos
        /// </summary>
        /// <param name="email">Objeto con los datos del email recibido en el cuerpo de la petición POST</param>
        /// <param name="sendGridMessage">Objeto de salida para enviar un email usando Sendgrid</param>
        /// <param name="log">Objeto para escribir en un log o consola</param>
        /// <returns></returns>
        [FunctionName("EmailSenderHttp")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "emails")] Email email,
            [SendGrid(ApiKey ="SendGridApiKey")] out SendGridMessage sendGridMessage,
            ILogger log)
        {
            sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom("no-reply@3ysolutions.com");
            sendGridMessage.AddTo(email.To);
            sendGridMessage.AddContent("text/html", email.Body);
            sendGridMessage.SetSubject(email.Subject);

            return new OkObjectResult($"Mensaje de email enviado a {email.To}");
        }
    }

    public class Email
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
