using Azure;
using Azure.Communication.Email;
using System;
using System.Threading.Tasks;

namespace AzureCommunicationServices.SendEmail
{
    class SendEmail
    {
        static async Task Main(string[] args)
        {
            // This code is for sending email using Azure Communication Services.
            var connectionString = "<CONNECTION_STRING>";
            var emailClient = new EmailClient(connectionString);

            var sender = "<SENDER_EMAIL>";
            var recipient = "<RECIPIENT_EMAIL>";
            var subject = "Send email";
            var htmlContent = "<html><body><h1>Quick send email test</h1><br/><h4>Communication email as a service mail send app working properly</h4><p>Happy Learning!!</p></body></html>";

            try
            {
                var emailSendOperation = await emailClient.SendAsync(
                    wait: WaitUntil.Completed,
                    senderAddress: sender, // The email address of the domain registered with the Communication Services
                    recipientAddress: recipient,
                    subject: subject,
                    htmlContent: htmlContent);
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                /// Get the OperationId so that it can be used for tracking the message for troubleshooting
                string operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }
    }
}