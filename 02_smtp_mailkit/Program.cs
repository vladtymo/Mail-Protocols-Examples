using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace _02_smtp_mailkit
{
    internal class Program
    {
        // ! change the credentials and addresses
        const string username = "<your_email>";     // change here
        const string password = "<your_password>";  // change here

        static void Main(string[] args)
        {
            /////////////// Send Mails (SMTP)
            Console.Write("Enter address to send an email: ");
            string to = Console.ReadLine();

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Vlad", username));  // change here
            message.To.Add(new MailboxAddress("Test User", to)); // change here
            message.Subject = "Добрий вечір, ми з України! How you doin?";
            message.Importance = MessageImportance.High;

            message.Body = new TextPart("plain")
            {
                Text = @"Hey Alice,

            What are you up to this weekend? Monica is throwing one of her parties on
            Saturday and I was hoping you could make it.

            Will you be my +1?

            -- Joey 
            "
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);

                client.Authenticate(username, password);

                //var options = FormatOptions.Default.Clone();

                //if (client.Capabilities.HasFlag(SmtpCapabilities.UTF8))
                //    options.International = true;

                client.Send(message);
            }
        }
    }
}