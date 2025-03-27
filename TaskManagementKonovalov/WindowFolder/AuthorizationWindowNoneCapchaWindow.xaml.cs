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
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;

namespace TaskManagementKonovalov.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindowNoneCapchaWindow.xaml
    /// </summary>
    public partial class AuthorizationWindowNoneCapchaWindow : Window
    {
        public AuthorizationWindowNoneCapchaWindow()
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
        }

        private bool SaveFileNull = true;
        private bool lightMode = true;
        private bool SaveMe = false;
        private string LoginSave;
        private string PasswordSave;
        private string path;
        private string pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryLight.xaml";
        private int faille = 0;

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
            else
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
                    using (StreamWriter newTask = new StreamWriter(path + "Save.txt", false))
                    {
                        newTask.WriteLine("");
                    }
                    Login();
                }
                return;
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
                        faille = faille + 1;

                        if (faille >= 3)
                        {
                            Capcha();
                        }

                        MBClass.ErrorMB("Введен не правильный логин или пароль");
                        LoginTB.Focus();
                        return;
                    }
                    if (user.PasswordUser != PasswordSave)
                    {
                        faille = faille + 1;

                        if (faille >= 3)
                        {
                            Capcha();
                        }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(0.01) };
            timer.Start();
            timer.Tick += (senders, args) =>
            {
                timer.Stop();
                using (StreamReader newTask = new StreamReader(path + "Save.txt", true))
                {
                    if (newTask == null || newTask.ReadLine() == "" ||
                        newTask.ReadLine() == " ")
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
                    using (StreamReader newTask = new StreamReader(path + "Save.txt", false))
                    {
                        LoginSave = newTask.ReadLine();
                        PasswordSave = System.IO.File.ReadLines(path + "Save.txt").Skip(1).First();

                        LoginTB.Text = LoginSave;
                        PasswordPB.Password = PasswordSave;
                        SaveMeCb.IsChecked = true;

                        //Login();
                    }
                }
            };
        }
    }
}
