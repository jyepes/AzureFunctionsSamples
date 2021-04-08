using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace AzureFunctionsSamples
{
    public static class EmailSenderQueue
    {

        /// <summary>
        /// Escucha una Queue y envía un email a través de Sendgrid usando los datos que llegan en la Queue
        /// </summary>
        /// <param name="email">Objeto recibido en la Eueue con los datos del email</param>
        /// <param name="sendGridMessage">Objeto para enviar un email usando Sendgrid</param>
        /// <param name="log">Objeto para escribir en un log o consola</param>
        [FunctionName("EmailSenderQueue")]
        public static void Run([QueueTrigger("email-queue", Connection = "AzureWebJobsStorage")]Email email,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage sendGridMessage,
            ILogger log)
        {
            sendGridMessage = new SendGridMessage();
            sendGridMessage.SetFrom("no-reply@3ysolutions.com");
            sendGridMessage.AddTo(email.To);
            sendGridMessage.AddContent("text/html", email.Body);
            sendGridMessage.SetSubject(email.Subject);
        }
    }
}
