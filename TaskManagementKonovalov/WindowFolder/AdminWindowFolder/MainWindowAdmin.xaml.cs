using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using TaskManagementKonovalov.PageFolder;
using TaskManagementKonovalov.PageFolder.AdminPageFolder;

namespace TaskManagementKonovalov.WindowFolder.AdminWindowFolder
{
    /// <summary>
    /// Логика взаимодействия для MainWindowAdmin.xaml
    /// </summary>
    public partial class MainWindowAdmin : Window
    {
        Staff staff = new Staff();

        public MainWindowAdmin(bool settings)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            if (settings == true)
            {
                MainFrame.Navigate(new SettingsPage("MainWindowStaff", this));
            }
            else
            {
                MainFrame.Navigate(new ListStaffPage());
            }

            MenuGr.Margin = new Thickness(1000);
            CloseMenuIm.Margin = new Thickness(1000);

            string deptName = (App.Current as App).DeptName;

            staff = DBEntities.GetContext().Staff.FirstOrDefault
                (s => s.User.LoginUser == deptName);

            if (staff.PhotoStaff != null)
            {
                var photo = ClassImage.ConvertByteArrayToImage(staff.PhotoStaff);
                System.Drawing.Image image = ClassImage.ConvertToImage(photo);
                var imageFix = ClassImage.FixedSize(image, 130, 130, true);

                AvatarIm.ImageSource = ClassImage.ConvertToBitmapImage(imageFix);
                CopyAvatarIm.ImageSource = ClassImage.ConvertToBitmapImage(imageFix);

                PersonalIm.Margin = new Thickness(1000);
                CopyPersonalIm.Margin = new Thickness(1000);
            }
            else
            {
                string themeNow = (App.Current as App).ThemeNow;

                if (themeNow == "Dark")
                {
                    AvatarIm.ImageSource = new BitmapImage
                        (new Uri(path + @"/ResourceFolder/ImageFolder/DarkThemeImageFolder/EllipseDark.png", UriKind.Relative));
                    CopyAvatarIm.ImageSource = new BitmapImage
                        (new Uri(path + @"/ResourceFolder/ImageFolder/DarkThemeImageFolder/EllipseDark.png", UriKind.Relative));
                }
                else if (themeNow == "Light")
                {
                    AvatarIm.ImageSource = new BitmapImage
                        (new Uri(path + @"/ResourceFolder/ImageFolder/LightThemeImageFolder/EllipseLight.png", UriKind.Relative));
                    CopyAvatarIm.ImageSource = new BitmapImage
                        (new Uri(path + @"/ResourceFolder/ImageFolder/LightThemeImageFolder/EllipseLight.png", UriKind.Relative));
                }
            }

            FerstNameLb.Content = staff.LastNameStaff.ToString();
            LasteNameLb.Content = staff.FirstNameStaff.ToString();
            MiddleNameLb.Content = staff.MiddleNameStaff.ToString();

            CopyFullNameLb.Content = staff.LastNameStaff.ToString() + "\n" +
                staff.FirstNameStaff.ToString() + "\n" +
                staff.MiddleNameStaff.ToString();
        }

        private string path;

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseIm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool ret = MBClass.QestionMB("Вы действительно желаете выйти?");
            if (ret == true)
            {
                this.Close();
            }
        }

        private void ListStaffBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ListStaffPage());
        }

        private void AvatarEl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuGr.Margin = new Thickness(0);
            CloseMenuIm.Margin = new Thickness(0);
        }

        private void PersonalIm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuGr.Margin = new Thickness(0);
            CloseMenuIm.Margin = new Thickness(0);
        }

        private void CloseMenuIm_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MenuGr.Margin = new Thickness(1000);
            CloseMenuIm.Margin = new Thickness(1000);
        }

        private void InformationPersonalBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ViewingProfilePage());

            MenuGr.Margin = new Thickness(1000);
            CloseMenuIm.Margin = new Thickness(1000);
        }

        private void SettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SettingsPage("MainWindowAdmin", this));

            MenuGr.Margin = new Thickness(1000);
            CloseMenuIm.Margin = new Thickness(1000);
        }

        private void ChangeUserBtn_Click(object sender, RoutedEventArgs e)
        {
            bool ret = MBClass.QestionMB("Вы действительно желаете выйти в окно авторизации?");
            if (ret == true)
            {
                new AuthorizationWindowNoneCapchaWindow().Show();
                this.Close();
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.MainFrame.CanGoBack)
            {
                /*if (this.MainFrame.Navigate(new ListStaffPage()))
                {
                    MainFrame.Navigate(new ListVeterinaryEquipmentPage());
                }
                else
                {*/
                this.MainFrame.GoBack();
                //}
            }
            else
            {
                bool ret = MBClass.QestionMB("Вы действительно желаете выйти в окно авторизации?");
                if (ret == true)
                {
                    new AuthorizationWindowNoneCapchaWindow().Show();
                    this.Close();
                }
            }
        }

        private void TaskBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HistoryTaskBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReportBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
