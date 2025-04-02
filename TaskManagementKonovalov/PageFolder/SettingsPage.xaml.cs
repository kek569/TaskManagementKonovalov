using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using TaskManagementKonovalov.WindowFolder.AdminWindowFolder;
using TaskManagementKonovalov.WindowFolder.StaffWindowFolder;

namespace TaskManagementKonovalov.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        Staff staff = new Staff();

        public SettingsPage(string windowName, Window window)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            ThemeCb.Items.Add("По умолчанию");
            ThemeCb.Items.Add("Тёмная");
            ThemeCb.Items.Add("Светлая");
            ThemeCb.Items.Add("Выбирите тему...");

            ThemeCb.SelectedIndex = 3;

            windowsName = windowName;
            windows = window;

            string deptName = (App.Current as App).DeptName;

            staff = DBEntities.GetContext().Staff.FirstOrDefault
            (s => s.User.LoginUser == deptName);

            RenameLoginTb.Text = staff.User.LoginUser;
            login = staff.User.LoginUser;

            RenameNumberPhoneTb.Text = staff.NumberPhoneStaff;
            numberPhone = staff.NumberPhoneStaff;
        }

        private string path;
        private string pathDictionary;
        private string themeSetting = (App.Current as App).ThemeSettings;
        private string windowsName;
        private string login;
        private string numberPhone;
        private bool lightMode = true;
        private Window windows;

        private void ThemeCb_MouseEnter(object sender, MouseEventArgs e)
        {
            ThemeCb.Items.Remove("Выбирите тему...");

            if (themeSetting == "Default")
            {
                ThemeCb.SelectedIndex = 0;
            }
            else if (themeSetting == "Dark")
            {
                ThemeCb.SelectedIndex = 1;
            }
            else if (themeSetting == "Light")
            {
                ThemeCb.SelectedIndex = 2;
            }
        }

        private void ThemeCb_MouseLeave(object sender, MouseEventArgs e)
        {
            ThemeCb.Items.Add("Выбирите тему...");

            ThemeCb.SelectedIndex = 3;
        }

        private void ThemeCb_DropDownClosed(object sender, EventArgs e)
        {
            if (themeSetting != "Default" &&
                ThemeCb.SelectedIndex == 0)
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

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SavePathDictionary.txt", false))
                {
                    newTask.WriteLine("");
                }

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SaveThemeSettings.txt", false))
                {
                    newTask.WriteLine("");
                }

                Restart();
            }
            else if (themeSetting != "Dark" &&
                ThemeCb.SelectedIndex == 1)
            {
                pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryDark.xaml";
                (App.Current as App).PathDictionary = pathDictionary;

                (App.Current as App).ThemeSettings = "Dark";
                (App.Current as App).ThemeNow = "Dark";

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SavePathDictionary.txt", false))
                {
                    newTask.WriteLine(pathDictionary);
                }

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SaveThemeSettings.txt", false))
                {
                    newTask.WriteLine("Dark");
                }

                Restart();
            }
            else if (themeSetting != "Light" &&
                ThemeCb.SelectedIndex == 2)
            {
                pathDictionary = "pack://application:,,,/ResourceFolder/DictionaryLight.xaml";
                (App.Current as App).PathDictionary = pathDictionary;

                (App.Current as App).ThemeSettings = "Light";
                (App.Current as App).ThemeNow = "Light";

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SavePathDictionary.txt", false))
                {
                    newTask.WriteLine(pathDictionary);
                }

                using (StreamWriter newTask = new StreamWriter(path + "SaveFolder\\SaveThemeSettings.txt", false))
                {
                    newTask.WriteLine("Light");
                }

                Restart();
            }
        }

        private void Restart()
        {
            if (windowsName == "MainWindowStaff")
            {
                new MainWindowStaff(true).Show();
                windows.Close();
            }
            else if (windowsName == "MainWindowAdmin")
            {
                new MainWindowAdmin(true).Show();
                windows.Close();
            }
        }

        private void RenameLoginBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameLoginTb.Text = "Изменить логин";
        }

        private void RenameLoginBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameLoginTb.Text = login;
        }

        private void RenameNumberPhoneBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            RenameNumberPhoneTb.Text = "Изменить номер\nтелефона";
        }

        private void RenameNumberPhoneBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            RenameNumberPhoneTb.Text = numberPhone;
        }

        private void RenameLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RenameLoginPage(windowsName, windows));
        }

        private void RenamePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RenamePasswordPage(windowsName, windows));
        }

        private void RenameNumberPhoneBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RenameNumberPhonePage(windowsName, windows));
        }

        private void SendsBugsBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SendsBugsPage(windowsName, windows));
        }

        private void ChangePhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ChangePhotoPage(windowsName, windows));
        }
    }
}
