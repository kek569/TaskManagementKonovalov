using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RenameNumberPhonePage.xaml
    /// </summary>
    public partial class RenameNumberPhonePage : System.Windows.Controls.Page
    {
        Staff staff = new Staff();

        public RenameNumberPhonePage(string windowName, System.Windows.Window window)
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
                (s => s.NumberPhoneStaff == deptName);

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
                (s => s.NumberPhoneStaff == NumberPhoneTb.Text);

            if (string.IsNullOrWhiteSpace(NumberPhoneTb.Text))
            {
                MBClass.ErrorMB("Введите номер телефона");
                NumberPhoneTb.Focus();
            }
            else if (NumberPhoneTb.Text == staff.NumberPhoneStaff)
            {
                MBClass.ErrorMB("Введите новый номер телефона");
                NumberPhoneTb.Focus();
            }
            else if (staffs != null)
            {
                if (staffs.NumberPhoneStaff == NumberPhoneTb.Text)
                {
                    MBClass.ErrorMB("Данный номер телефона уже привязан к другому");
                    NumberPhoneTb.Focus();
                }
                else
                {
                    staff.NumberPhoneStaff = NumberPhoneTb.Text;
                    DBEntities.GetContext();

                    MBClass.InfoMB("Данные о номоре телефона успешно изменены");
                    NavigationService.Navigate(new SettingsPage(windowsName, windows));
                }
            }
            else
            {
                staff.NumberPhoneStaff = NumberPhoneTb.Text;
                DBEntities.GetContext().SaveChanges();

                MBClass.InfoMB("Данные о номере телефона успешно изменены");
                NavigationService.Navigate(new SettingsPage(windowsName, windows));
            }
        }

        private void NumberPhoneTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+()-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
