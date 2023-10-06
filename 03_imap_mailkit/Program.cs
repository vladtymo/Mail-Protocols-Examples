using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MimeKit;
using System;
using System.Security.Cryptography;
using System.Text;

namespace _03_imap_mailkit
{
    internal class Program
    {
        // ! change the credentials and addresses
        const string username = "tmvlad33@gmail.com"; // change here
        const string password = "gxknljmktrlthlyx"; // change here

        static void Main(string[] args)
        {
            ///////// Get Mails (IMAP)
            Console.OutputEncoding = Encoding.UTF8;

            using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

                client.Authenticate(username, password);

                //foreach (var fl in client.GetFolders(client.PersonalNamespaces[0]))
                //{
                //    Console.WriteLine(fl.Name);
                //}

                var sendFolder = client.GetFolder(SpecialFolder.Sent);

                sendFolder.Open(FolderAccess.ReadWrite);

                //foreach (var item in sendFolder.Search(SearchQuery.All))
                //{
                //    var mail = sendFolder.GetMessage(item);

                //    Console.WriteLine(mail.Subject);
                //}
                
                var inbox = client.Inbox;

                inbox.Open(FolderAccess.ReadWrite);

                foreach (var i in inbox.Search(SearchQuery.DeliveredAfter(DateTime.Today)))
                {
                    var m = inbox.GetMessage(i);

                    Console.WriteLine($"Mail: {m.Subject}");
                }

                // ------------------------------------------------------------------------------

                // --------------- get all folders
                //foreach (var item in client.GetFolders(client.PersonalNamespaces[0]))
                //{
                //    Console.WriteLine("Folder: " + item.Name);
                //}

                //// -------------- get all sent messages
                var folder = client.GetFolder(SpecialFolder.Sent);
                //folder.Open(FolderAccess.ReadWrite);

                //IList<UniqueId> uids = folder.Search(SearchQuery.All);

                //Console.WriteLine("--------- Sent Mailbox:");
                //foreach (var i in uids)
                //{
                //    MimeMessage message = folder.GetMessage(i);
                //    Console.WriteLine($"{message.Date}: {message.Subject} - {new string(message.TextBody?.Take(10).ToArray())}...");
                //}

                ////// -------------------- show Inbox 
                //client.Inbox.Open(FolderAccess.ReadOnly);

                //Console.WriteLine("--------- Inbox:");
                //foreach (var uid in client.Inbox.Search(SearchQuery.All))
                //{
                //    var m = client.Inbox.GetMessage(uid);
                //    // show message details
                //    Console.WriteLine($"Mail: {m.Subject} - {new string(m.TextBody.Take(10).ToArray())}...");
                //}

                //// ---------------------- delete message
                folder.Open(FolderAccess.ReadWrite);
                var id = folder.Search(SearchQuery.All).FirstOrDefault();
                var mail = folder.GetMessage(id);

                Console.WriteLine(mail.Date + " " + mail.Subject);

                folder.AddFlags(new UniqueId[] { id }, MessageFlags.Deleted, false);

                folder.Expunge();

                folder.MoveTo(id, client.GetFolder(SpecialFolder.Junk));  // move to spam
                folder.AddFlags(id, MessageFlags.Seen, false);            // mark as read
                folder.MoveTo(id, client.GetFolder(SpecialFolder.Trash)); // delete mail

                Console.WriteLine("Press to exit!");
                Console.ReadKey();

            } // Dispose()
        }
    }
}