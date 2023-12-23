using MailKit.Net.Imap;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MailClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string username = "vlad.tmsh@gmail.com"; // change here
        const string password = "tkifcinpvlcrygle"; // change here

        private ImapClient client = new();

        public MainWindow()
        {
            InitializeComponent();

            client.Connect("imap.gmail.com", 993, SecureSocketOptions.SslOnConnect);

            client.Authenticate(username, password);

            foreach (var fl in client.GetFolders(client.PersonalNamespaces[0]))
            {
                folderList.Items.Add(fl.Name);
            }
        }
    }
}
