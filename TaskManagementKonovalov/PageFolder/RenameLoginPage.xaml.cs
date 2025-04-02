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
    /// Логика взаимодействия для RenameLoginPage.xaml
    /// </summary>
    public partial class RenameLoginPage : System.Windows.Controls.Page
    {
        Staff staff = new Staff();

        public RenameLoginPage(string windowName, System.Windows.Window window)
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
            Staff staffs = new Staff();
            staffs = DBEntities.GetContext().Staff.FirstOrDefault
                (s => s.User.LoginUser == LoginTb.Text);

            if (string.IsNullOrWhiteSpace(LoginTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTb.Focus();
            }
            else if (LoginTb.Text == staff.User.LoginUser)
            {
                MBClass.ErrorMB("Введите новый логин");
                LoginTb.Focus();
            }
            else if (staffs != null)
            {
                if (staffs.User.LoginUser == LoginTb.Text)
                {
                    MBClass.ErrorMB("Данный логин уже существует");
                    LoginTb.Focus();
                }
                else
                {
                    staff.User.LoginUser = LoginTb.Text;
                    DBEntities.GetContext();

                    using (StreamWriter newTask = new StreamWriter(path + "Save.txt", false))
                    {
                        newTask.WriteLine("");
                    }

                    MBClass.InfoMB("Данные о логине успешно изменены");
                    NavigationService.Navigate(new SettingsPage(windowsName, windows));
                }
            }
            else
            {
                staff.User.LoginUser = LoginTb.Text;
                DBEntities.GetContext().SaveChanges();

                using (StreamWriter newTask = new StreamWriter(path + "Save.txt", false))
                {
                    newTask.WriteLine("");
                }

                MBClass.InfoMB("Данные о логине успешно изменены");
                NavigationService.Navigate(new SettingsPage(windowsName, windows));
            }
        }
    }
}
