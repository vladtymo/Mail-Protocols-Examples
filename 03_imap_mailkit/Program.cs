using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using System.Text;

namespace _03_imap_mailkit
{
    internal class Program
    {
        // ! change the credentials and addresses
        const string username = "prodoq@gmail.com"; // change here
        const string password = "kznklnglnraujhbi"; // change here

        static void Main(string[] args)
        {
            ///////// Get Mails (IMAP)
            Console.OutputEncoding = Encoding.UTF8;

            using (var client = new ImapClient())
            {
                //client.Alert += Client_Alert;
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(username, password);

                foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
                {
                    Console.WriteLine("Folder: " + item.Name);
                }

                //////// Show Inbox 
                client.Inbox.Open(FolderAccess.ReadOnly);
                var uids = client.Inbox.Search(SearchQuery.All); // get all mails

                foreach (var uid in uids)
                {
                    var m = client.Inbox.GetMessage(uid);
                    // show message subject
                    Console.WriteLine($"Mail: {m.Subject} - {new string(m.TextBody.Take(50).ToArray())}...");
                }

                //////// Delete message
                client.GetFolder(SpecialFolder.Sent).Open(FolderAccess.ReadWrite);

                var folder = client.GetFolder(SpecialFolder.Sent);

                var id = folder.Search(SearchQuery.All)[0];
                var mail = folder.GetMessage(id);

                Console.WriteLine(mail.Date + " " + mail.Subject);

                folder.MoveTo(id, client.GetFolder(SpecialFolder.Junk));
                folder.AddFlags(id, MessageFlags.Deleted, true);

                Console.WriteLine("Press to exit!");
                Console.ReadKey();

                client.Disconnect(true);
            }
        }
    }
}