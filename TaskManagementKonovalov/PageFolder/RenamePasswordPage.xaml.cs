using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
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

namespace TaskManagementKonovalov.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для RenamePasswordPage.xaml
    /// </summary>
    public partial class RenamePasswordPage : System.Windows.Controls.Page
    {
        Staff staff = new Staff();

        public RenamePasswordPage(string windowName, System.Windows.Window window)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            string deptName = (App.Current as App).DeptName;

            staff = DBEntities.GetContext().Staff.FirstOrDefault
                (s => s.User.LoginUser == deptName);

            windowsName = windowName;
            windows = window;
        }

        private string path;
        private string windowsName;
        private System.Windows.Window windows;

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(OldPasswordPb.Password))
            {
                MBClass.ErrorMB("Введите старый пароль");
                OldPasswordPb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(NewPasswordPb.Password))
            {
                MBClass.ErrorMB("Введите новый пароль");
                NewPasswordPb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(NewPasswordAgainPb.Password))
            {
                MBClass.ErrorMB("Введите новый пароль повторно");
                NewPasswordAgainPb.Focus();
            }
            else if (OldPasswordPb.Password != staff.User.PasswordUser)
            {
                MBClass.ErrorMB("Введен неправильный старый пароль");
                OldPasswordPb.Focus();
            }
            else if (OldPasswordPb.Password == NewPasswordPb.Password)
            {
                MBClass.ErrorMB("Введен старый пароль, введите новый пароль");
                NewPasswordPb.Focus();
            }
            else if (NewPasswordPb.Password != NewPasswordAgainPb.Password)
            {
                MBClass.ErrorMB("Введеный повторный пароль не совпадает с новым паролем");
                NewPasswordAgainPb.Focus();
            }
            else
            {
                staff.User.PasswordUser = NewPasswordPb.Password;
                DBEntities.GetContext().SaveChanges();

                using (StreamWriter newTask = new StreamWriter(path + "Save.txt", false))
                {
                    newTask.WriteLine("");
                }

                MBClass.InfoMB("Данные о пароле успешно изменены");
                NavigationService.Navigate(new SettingsPage(windowsName, windows));
            }
        }
    }
}
