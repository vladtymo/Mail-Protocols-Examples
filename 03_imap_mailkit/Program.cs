using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System.Text;

namespace _03_imap_mailkit
{
    internal class Program
    {
        // ! change the credentials and addresses
        const string username = "<your_email>"; // change here
        const string password = "<your_password>"; // change here

        static void Main(string[] args)
        {
            ///////// Get Mails (IMAP)
            Console.OutputEncoding = Encoding.UTF8;

            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(username, password);

                // --------------- get all folders
                foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
                {
                    Console.WriteLine("Folder: " + item.Name);
                }

                // -------------- get all sent messages
                var folder = client.GetFolder(SpecialFolder.Sent);
                folder.Open(FolderAccess.ReadWrite);

                IList<UniqueId> uids = folder.Search(SearchQuery.All);

                foreach (var i in uids)
                {
                    MimeMessage message = folder.GetMessage(i);
                    Console.WriteLine($"{message.Date}: {message.Subject} - {new string(message.TextBody.Take(10).ToArray())}...");
                }

                // -------------------- show Inbox 
                client.Inbox.Open(FolderAccess.ReadOnly);

                foreach (var uid in client.Inbox.Search(SearchQuery.All))
                {
                    var m = client.Inbox.GetMessage(uid);
                    // show message details
                    Console.WriteLine($"Mail: {m.Subject} - {new string(m.TextBody.Take(10).ToArray())}...");
                }

                // ---------------------- delete message
                var id = folder.Search(SearchQuery.All).FirstOrDefault();
                var mail = folder.GetMessage(id);

                Console.WriteLine(mail.Date + " " + mail.Subject);

                folder.MoveTo(id, client.GetFolder(SpecialFolder.Junk)); // move to spam
                folder.AddFlags(id, MessageFlags.Deleted, true);

                Console.WriteLine("Press to exit!");
                Console.ReadKey();

                client.Disconnect(true);
            }
        }
    }
}