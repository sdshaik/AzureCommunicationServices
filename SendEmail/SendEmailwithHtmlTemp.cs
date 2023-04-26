using Azure;
using Azure.Communication.Email;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AzureCommunicationServices.SendEmail
{
    class SendEmailwithHtmlTemp
    {
        static async Task Main(string[] args)
        {
            var connectionString = "";
            var emailClient = new EmailClient(connectionString);

            var sender = "";
            var recipientName = "Testing"; // set recipient name as a parameter
            var recipientEmail = "Testing@gmail.com"; // set recipient email as a parameter
            var subject = "Send email with HTML template file";
            var link = "Google.com";

            string mailbody =  PopulateBodyForProfessorActivity(recipientName, link);

            var emailContent = new EmailContent(subject)
            {
                Html = mailbody
            };

            var emailMessage = new EmailMessage(sender, recipientEmail, emailContent);

            var emailSendOperation = await emailClient.SendAsync(
                wait: WaitUntil.Started,
                message: emailMessage);

            // Call UpdateStatus on the email send operation to poll for the status manually.
            try
            {
                while (true)
                {
                    await emailSendOperation.UpdateStatusAsync();
                    if (emailSendOperation.HasCompleted)
                    {
                        break;
                    }
                    await Task.Delay(100);
                }

                if (emailSendOperation.HasValue)
                {
                    Console.WriteLine($"Email queued for delivery. Status = {emailSendOperation.Value.Status}");
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Email send failed with Code = {ex.ErrorCode} and Message = {ex.Message}");
            }

            // Get the OperationId so that it can be used for tracking the message for troubleshooting
            string operationId = emailSendOperation.Id;
            Console.WriteLine($"Email operation id = {operationId}");
        }

        private static string PopulateBodyForProfessorActivity(string userName, string link)
        {
            string body = string.Empty;
              string fileFullPath = string.Format(@".\pathofhtmfile\sendemail.html");
          //  var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Email", "emailtemp.html");
            using (StreamReader reader = System.IO.File.OpenText(fileFullPath)) { body = reader.ReadToEnd(); }
            body = body.Replace("{{recipientName}}", userName);
            return body;
        }
    }
    }
