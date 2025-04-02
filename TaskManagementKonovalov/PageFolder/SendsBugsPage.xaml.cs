using TaskManagementKonovalov.ClassFolder;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
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
using TaskManagementKonovalov.DataFolder;

namespace TaskManagementKonovalov.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для SendsBugsPage.xaml
    /// </summary>
    public partial class SendsBugsPage : System.Windows.Controls.Page
    {
        public SendsBugsPage(string windowName, System.Windows.Window window)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            windowsName = windowName;
            windows = window;
        }

        private void LoadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhoto();
        }
        
        string selectedFileOneName = "";
        string selectedFileTwoName = "";
        private string path;
        private string theme = "Отчет об ошибках";
        private string windowsName;
        private bool TwoImage = false;
        private System.Windows.Window windows;
        AlternateView jpeg_view;

        private void AddPhoto()
        {
            try
            {

                OpenFileDialog op = new OpenFileDialog();
                op.InitialDirectory = "";
                op.Filter = "All support graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*jpeg)|*.jpg;*jpeg|" +
                    "Portable Network Graphic (*.png|*.png";

                if (op.ShowDialog() == true)
                {
                    if (ImageOneIm.Source == null)
                    {
                        selectedFileOneName = op.FileName;
                        var bitmapImage = new BitmapImage();

                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(selectedFileOneName, UriKind.RelativeOrAbsolute);
                        bitmapImage.EndInit();

                        ImageOneIm.Source = bitmapImage;
                    }
                    else
                    {
                        selectedFileTwoName = op.FileName;
                        var bitmapImage = new BitmapImage();

                        bitmapImage.BeginInit();
                        bitmapImage.UriSource = new Uri(selectedFileTwoName, UriKind.RelativeOrAbsolute);
                        bitmapImage.EndInit();

                        ImageTwoIm.Source = bitmapImage;

                        TwoImage = true;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void SendingBugsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (selectedFileOneName == "" || selectedFileTwoName == "")
            {
                string smtpServer = " smtp.mail.ru"; //smpt сервер(зависит от почты отправителя)
                int smtpPort = 25; // Обычно используется порт 587 для TLS
                string smtpUsername = "bugssend@mail.ru"; //твоя почта, с которой отправляется сообщение
                string smtpPassword = "V6EX7Sc6n2ZXk6zJqbWm";//пароль приложения (от почты)

                // Создаем объект клиента SMTP
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    // Настройки аутентификации
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        string msg = MassageBugsTb.Text + @"<br>";
                        AlternateView html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");

                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add("bugssend@mail.ru");
                        mailMessage.Subject = theme;
                        try
                        {
                            // Отправляем сообщение
                            mailMessage.AlternateViews.Add(html_view);
                            smtpClient.Send(mailMessage);

                            MBClass.InfoMB("Сообщение об ошибке успешно отправлено");

                            NavigationService.Navigate(new SettingsPage(windowsName, windows));
                        }
                        catch (Exception ex)
                        {
                            MBClass.ErrorMB($"Ошибка отправки сообщения: {ex.Message}");
                        }
                    }
                }
            }
            else if (TwoImage == false)
            {
                string smtpServer = " smtp.mail.ru"; //smpt сервер(зависит от почты отправителя)
                int smtpPort = 25; // Обычно используется порт 587 для TLS
                string smtpUsername = "bugssend@mail.ru"; //твоя почта, с которой отправляется сообщение
                string smtpPassword = "V6EX7Sc6n2ZXk6zJqbWm";//пароль приложения (от почты)

                // Создаем объект клиента SMTP
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    // Настройки аутентификации
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        string msg = MassageBugsTb.Text + @"<br> <img src=""cid:uniqueId"" width=""*"" height=""*"">";
                        AlternateView html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");

                        if (selectedFileTwoName == "")
                        {
                            jpeg_view = new AlternateView(selectedFileOneName, MediaTypeNames.Image.Jpeg);
                        }
                        else if (selectedFileOneName == "")
                        {
                            jpeg_view = new AlternateView(selectedFileTwoName, MediaTypeNames.Image.Jpeg);
                        }

                        jpeg_view.ContentId = "uniqueId";
                        jpeg_view.TransferEncoding = TransferEncoding.Base64;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add("bugssend@mail.ru");
                        mailMessage.Subject = theme;
                        try
                        {
                            // Отправляем сообщение
                            mailMessage.AlternateViews.Add(html_view);
                            mailMessage.AlternateViews.Add(jpeg_view);
                            smtpClient.Send(mailMessage);

                            MBClass.InfoMB("Сообщение об ошибке успешно отправлено");

                            NavigationService.Navigate(new SettingsPage(windowsName, windows));
                        }
                        catch (Exception ex)
                        {
                            MBClass.ErrorMB($"Ошибка отправки сообщения: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                string smtpServer = " smtp.mail.ru"; //smpt сервер(зависит от почты отправителя)
                int smtpPort = 25; // Обычно используется порт 587 для TLS
                string smtpUsername = "bugssend@mail.ru"; //твоя почта, с которой отправляется сообщение
                string smtpPassword = "V6EX7Sc6n2ZXk6zJqbWm";//пароль приложения (от почты)

                // Создаем объект клиента SMTP
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    // Настройки аутентификации
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    using (MailMessage mailMessage = new MailMessage())
                    {
                        string msg = MassageBugsTb.Text + @"<br> <img src=""cid:uniqueOneId"" width=""*"" height=""*"">  
                                                            <br> <img src=""cid:uniqueTwoId"" width=""*"" height=""*"">";
                        AlternateView html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");

                        AlternateView jpeg_viewOne = new AlternateView(selectedFileOneName, MediaTypeNames.Image.Jpeg);
                        AlternateView jpeg_viewTwo = new AlternateView(selectedFileTwoName, MediaTypeNames.Image.Jpeg);

                        jpeg_viewOne.ContentId = "uniqueOneId";
                        jpeg_viewOne.TransferEncoding = TransferEncoding.Base64;

                        jpeg_viewTwo.ContentId = "uniqueTwoId";
                        jpeg_viewTwo.TransferEncoding = TransferEncoding.Base64;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.From = new MailAddress(smtpUsername);
                        mailMessage.To.Add("bugssend@mail.ru");
                        mailMessage.Subject = theme;
                        try
                        {
                            // Отправляем сообщение
                            mailMessage.AlternateViews.Add(html_view);
                            mailMessage.AlternateViews.Add(jpeg_viewOne);
                            mailMessage.AlternateViews.Add(jpeg_viewTwo);
                            smtpClient.Send(mailMessage);

                            MBClass.InfoMB("Сообщение об ошибке успешно отправлено");

                            NavigationService.Navigate(new SettingsPage(windowsName, windows));
                        }
                        catch (Exception ex)
                        {
                            MBClass.ErrorMB($"Ошибка отправки сообщения: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void ImageOneIm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageOneIm.Source = null;
            selectedFileOneName = "";
        }

        private void ImageTwoIm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageTwoIm.Source = null;
            selectedFileTwoName = "";
            TwoImage = false;
        }
    }
}
