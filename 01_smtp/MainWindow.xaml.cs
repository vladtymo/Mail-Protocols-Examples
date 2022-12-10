using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace _01_smtp
{
    public partial class MainWindow : Window
    {
        // generate apps password
        // https://stackoverflow.com/questions/72547853/unable-to-send-email-in-c-sharp-less-secure-app-access-not-longer-available

        const string myMailAddress = "prodoq@gmail.com";
        const string accountPassword = "qdqsraprlombkfjt";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // create new mail
            MailMessage mail = new MailMessage(myMailAddress, toTxtBox.Text)
            {
                Subject = subjectTxtBox.Text,
                Body = $"<h1>My Mail Message from C#</h1><p>{bodyTxtBox.Text}</p>",
                IsBodyHtml = true,
                Priority = MailPriority.High
            };

            // add attachments
            var result = MessageBox.Show("Do you want to attach a file?", "Attach File", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                    mail.Attachments.Add(new Attachment(dialog.FileName));
            }

            // send mail message
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(myMailAddress, accountPassword),
                EnableSsl = true
            };

            client.Send(mail);
        }
    }
}
