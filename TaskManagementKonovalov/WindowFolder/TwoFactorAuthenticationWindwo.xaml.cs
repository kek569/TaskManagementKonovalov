using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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
using System.Windows.Shapes;
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;

namespace TaskManagementKonovalov.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для TwoFactorAuthenticationWindwo.xaml
    /// </summary>
    public partial class TwoFactorAuthenticationWindwo : Window
    {
        Staff staff = new Staff();
        User users = new User();

        public TwoFactorAuthenticationWindwo(User user, string type, Window window)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

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
                    Random random = new Random();
                    code = random.Next(1000, 9999);

                    staff = DBEntities.GetContext().Staff.FirstOrDefault(c => c.IdUser == user.IdUser);
                    email = staff.Email;

                    //текст сообщения
                    string msg = "Была попытка вход в систему, для прохождение дальше нужно ввести данный код подверждения: " + code +
                        "\nЕсли это не вы пытаетесь войти в систему пожалуйста собшите нам об этом!" + @"<br>";
                    html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");

                    mailMessage.IsBodyHtml = true; //используем html код в написании письма
                    mailMessage.From = new MailAddress(smtpUsername); //от какой почты отправлаем
                    mailMessage.To.Add(email); //на какую почту отправлаем
                    mailMessage.Subject = "Код подверждения"; //тема письма
                    try
                    {
                        // Отправляем сообщение
                        mailMessage.AlternateViews.Add(html_view);
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        MBClass.ErrorMB($"Error");
                    }
                }
            }

            windows = window;
            users = user;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool ret = MBClass.QestionMB("Вы действительно желаете выйти?");
            if (ret == true)
            {
                this.Close();
            }
        }

        private int code;
        private string path;
        private string email;
        private int types;
        Window windows;
        private AlternateView html_view;

        private void ConfirmationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckTb.Text == code.ToString())
            {
                if (types == 1)
                {
                    switch (users.IdRole)
                    {
                        /*case 1:
                            (App.Current as App).DeptName = users.LoginUser;
                            new AdminWindowFolder.MainWindowAdmin(false).Show();
                            this.Close();
                            windows.Close();
                            break;
                        case 2:
                            (App.Current as App).DeptName = users.LoginUser;
                            new ClientWindowFolder.MainWindowClient(false).Show();
                            this.Close();
                            windows.Close();
                            break;*/
                    }
                }
                else
                {
                    new AuthorizationWindowNoneCapchaWindow().Show();
                    this.Close();
                    windows.Close();
                }
            }
            else
            {
                MBClass.ErrorMB("Код был введен не верно");
            }
        }

        private void ResendBtn_Click(object sender, RoutedEventArgs e)
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
                    Random random = new Random();
                    code = random.Next(1000, 9999);

                    if (types == 1)
                    {
                        //текст сообщения
                        string msg = "Была попытка вход в систему, для прохождение дальше нужно ввести данный код подверждения: " + code +
                            "\nЕсли это не вы пытаетесь войти в систему пожалуйста собшите нам об этом!" + @"<br>";
                        html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");
                    }
                    else
                    {
                        string msg = "Подвердите почту, для подтверждение почты введите данный код подверждения: " + code + @"<br>";
                        html_view = AlternateView.CreateAlternateViewFromString(msg, null, "text/html");
                    }

                    mailMessage.IsBodyHtml = true; //используем html код в написании письма
                    mailMessage.From = new MailAddress(smtpUsername); //от какой почты отправлаем
                    mailMessage.To.Add(email); //на какую почту отправлаем
                    mailMessage.Subject = "Код подверждения"; //тема письма
                    try
                    {
                        // Отправляем сообщение
                        mailMessage.AlternateViews.Add(html_view);
                        smtpClient.Send(mailMessage);
                    }
                    catch (Exception ex)
                    {
                        MBClass.ErrorMB($"Error");
                    }
                }
            }
        }
    }
}
