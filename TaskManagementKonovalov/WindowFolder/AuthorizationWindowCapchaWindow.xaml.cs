using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;
using EasyCaptcha.Wpf;
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using static System.Net.Mime.MediaTypeNames;

namespace TaskManagementKonovalov.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindowCapchaWindow.xaml
    /// </summary>
    public partial class AuthorizationWindowCapchaWindow : Window
    {
        public AuthorizationWindowCapchaWindow()
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            path = path + "SaveFolder\\";
            string pathDir = path;
            (App.Current as App).Path = path;

            using (StreamReader newTask = new StreamReader(path + "SavePathDictionary.txt", false))
            {
                if (newTask == null || newTask.ReadLine() == "" ||
                    newTask.ReadLine() == "")
                {
                    SaveFileNull = true;
                }
                else
                {
                    SaveFileNull = false;
                }
            }
            if (SaveFileNull == false)
            {
                using (StreamReader newTask = new StreamReader(path + "SavePathDictionary.txt", false))
                {
                    this.Resources = new ResourceDictionary() { Source = new Uri(System.IO.File.ReadLines(path + "SavePathDictionary.txt").Skip(0).First()) };
                    (App.Current as App).PathDictionary = System.IO.File.ReadLines(path + "SavePathDictionary.txt").Skip(0).First();
                }

                using (StreamReader newTask = new StreamReader(path + "SaveThemeSettings.txt", false))
                {
                    (App.Current as App).ThemeSettings = System.IO.File.ReadLines(path + "SaveThemeSettings.txt").Skip(0).First();
                    (App.Current as App).ThemeNow = System.IO.File.ReadLines(path + "SaveThemeSettings.txt").Skip(0).First();
                }
            }
            else
            {
                (App.Current as App).ThemeSettings = "Default";
                //ShouldSystemUseDarkMode();
                lightMode = true;
                try
                {
                    var v = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
                    if (v != null && v.ToString() == "0")
                        lightMode = false;
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }

                if (lightMode == true)
                {
                    pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryLight.xaml";
                    Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
                    (App.Current as App).PathDictionary = pathDictionary;
                    (App.Current as App).ThemeNow = "Light";
                }
                else
                {
                    pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryDark.xaml";
                    Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
                    (App.Current as App).PathDictionary = pathDictionary;
                    (App.Current as App).ThemeNow = "Dark";
                }
            }
            InitializeComponent();

            CaptchaEs.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            text = CaptchaEs.CaptchaText;
        }

        private bool SaveFileNull = true;
        private bool lightMode = true;
        private bool SaveMe = false;
        private bool ActiveTime = false;
        private string LoginSave;
        private string PasswordSave;
        private string path;
        private string pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryLight.xaml";
        private string text;
        private double Time = 10;
        private double CopyTime = 10;
        private int faille = 0;
        private int Fail = 0;
        private int Disable = 0;

        [DllImport("UXTheme.dll", SetLastError = true, EntryPoint = "#138")]
        public static extern bool ShouldSystemUseDarkMode();

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool ret = MBClass.QestionMB("Вы действительно желаете выйти?");
            if (ret == true)
            {
                this.Close();
            }
        }

        private void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTB.Text))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTB.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PasswordPB.Password))
            {
                MBClass.ErrorMB("Введите пароль");
                PasswordPB.Focus();
                return;
            }
            else if (CheckTb.Text == "" || CheckTb == null)
            {
                MBClass.ErrorMB("Пройдите капчу");
                CheckTb.Focus();
                return;
            }
            else if (CheckTb.Text == text)
            {
                LoginSave = LoginTB.Text;
                PasswordSave = PasswordPB.Password;
                if (SaveMeCb.IsChecked == true)
                {
                    SaveMe = true;
                    Login();
                }
                else
                {
                    SaveMe = false;
                    using (StreamWriter newTask = new StreamWriter("Save.txt", false)) { }
                    Login();
                }
                return;
            }
            else
            {
                MBClass.ErrorMB("Капча не пройдено");
                CaptchaEs.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
                text = CaptchaEs.CaptchaText;
                CheckTb.Text = "";
                CheckTb.Focus();
                Fail = Fail + 1;
                if (Fail >= 1 && ActiveTime == false)
                {
                    ActiveTime = true;
                    CopyTime = (int)Time;
                    double time = (int)Time + 1;

                    LogInBtn.IsEnabled = false;
                    Disable = 1;
                    var timer = new DispatcherTimer
                    { Interval = TimeSpan.FromSeconds(time) };
                    timer.Start();
                    TextTime();
                    timer.Tick += (senders, args) =>
                    {
                        timer.Stop();

                        LogInBtn.Content = "Войти";
                        LogInBtn.IsEnabled = true;
                        Time = Time * 1.5;
                        Fail = 2;
                        Disable = 0;
                        ActiveTime = false;
                    };

                }
                return;
            }
        }

        private void TextTime()
        {
            if (CopyTime > 0.5)
            {
                int a = (int)CopyTime;
                var timer = new DispatcherTimer
                { Interval = TimeSpan.FromSeconds(1) };
                timer.Start();
                LogInBtn.Content = a;
                timer.Tick += (senders, args) =>
                {
                    timer.Stop();
                    CopyTime = CopyTime - 1;
                    TextTime();
                };
            }
        }

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(LoginSave))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTB.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PasswordSave))
            {
                MBClass.ErrorMB("Введите пароль");
                PasswordPB.Focus();
                return;
            }
            else
            {
                try
                {
                    var user = DBEntities.GetContext()
                        .User
                        .FirstOrDefault(u => u.LoginUser == LoginSave);

                    if (user == null)
                    {
                        CaptchaEs.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
                        CheckTb.Text = "";
                        text = CaptchaEs.CaptchaText;

                        MBClass.ErrorMB("Введен не правильный логин или пароль");
                        LoginTB.Focus();
                        return;
                    }
                    if (user.PasswordUser != PasswordSave)
                    {
                        CaptchaEs.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
                        CheckTb.Text = "";
                        text = CaptchaEs.CaptchaText;

                        MBClass.ErrorMB("Введен не правильный логин или пароль");
                        PasswordPB.Focus();
                        return;
                    }
                    else
                    {
                        if (SaveMe == true)
                        {
                            using (StreamWriter newTask = new StreamWriter(path + "Save.txt", false))
                            {
                                newTask.WriteLine(LoginSave + "\n" + PasswordSave);
                            }
                        }

                        switch (user.IdRole)
                        {
                            /*case 1:
                                (App.Current as App).DeptName = LoginSave;
                                new AdminWindowFolder.MainWindowAdmin(false).Show();
                                this.Close();
                                break;
                            case 2:
                                (App.Current as App).DeptName = LoginSave;
                                new StaffWindowFolder.MainWindowStaff(false).Show();
                                this.Close();
                                break;*/
                        }
                    }
                }
                catch (Exception ex)
                {
                    MBClass.ErrorMB(ex);
                }
            }
        }

        private void Capcha()
        {
            new AuthorizationWindowCapchaWindow().Show();
            this.Close();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            CaptchaEs.CreateCaptcha(Captcha.LetterOption.Alphanumeric, 5);
            CheckTb.Text = "";
            text = CaptchaEs.CaptchaText;
        }
    }
}
