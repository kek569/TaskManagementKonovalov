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

namespace TaskManagementKonovalov.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для ListStaffPage.xaml
    /// </summary>
    public partial class ListStaffPage : Page
    {
        public ListStaffPage()
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();
            StaffListB.ItemsSource = DBEntities.GetContext()
                        .Staff.ToList().OrderBy(s => s.IdStaff);
        }

        private string path;

        private void AddStaffMi_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddStaffPage());
        }

        private void EditStaffMi_Click(object sender, RoutedEventArgs e)
        {
            if (StaffListB.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите строку для редактирование");
            }
            else
            {
                NavigationService.Navigate
                    (new EditeStaffPage(StaffListB.SelectedItem as Staff));
            }
        }

        private void ResetPasswordStaffMi_Click(object sender, RoutedEventArgs e)
        {
            if (StaffListB.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите строку для сброса пороля");
            }
            else
            {
                bool ret = MBClass.QestionMB("Вы действительно хотите сбросить пароль данному сотруднику?");
                if (ret == true)
                {
                    Staff staff = StaffListB.SelectedItem as Staff;

                    staff = DBEntities.GetContext().Staff
                                .FirstOrDefault(s => s.IdStaff == staff.IdStaff);

                    string res = RandomPassword();

                    staff.User.PasswordUser = res.ToString();
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("Пароль был успешно сброшан на:\n" + res);
                    NavigationService.Navigate(new ListStaffPage());
                }
            }
        }

        private string RandomPassword()
        {
            int length = 8;

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
                
            }
            return res.ToString();
        }

        private void DeleteStafffMi_Click(object sender, RoutedEventArgs e)
        {
            if (StaffListB.SelectedItem == null)
            {
                MBClass.ErrorMB("Выберите строку для удаления");
            }
            else
            {
                bool ret = MBClass.QestionMB("Вы действительно хотите удалить данную строку?");
                if (ret == true)
                {
                    Staff staff = StaffListB.SelectedItem as Staff;

                    staff = DBEntities.GetContext().Staff
                                .FirstOrDefault(s => s.IdStaff == staff.IdStaff);

                    User user = new User();

                    user = DBEntities.GetContext().User
                                .FirstOrDefault(s => s.IdUser == staff.IdUser);

                    DBEntities.GetContext().User.Remove(user);
                    DBEntities.GetContext().Staff.Remove(staff);
                    DBEntities.GetContext().SaveChanges();

                    MBClass.InfoMB("Данные успешно были удалены!");
                    NavigationService.Navigate(new ListStaffPage());
                }
            }
        }

        private void UpdateStaffMi_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListStaffPage());
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                StaffListB.ItemsSource = DataFolder.DBEntities.GetContext().Staff.
                    Where(s => s.FirstNameStaff.StartsWith(SearchTb.Text) ||
                    s.FirstNameStaff.StartsWith(SearchTb.Text) ||
                    s.MiddleNameStaff.StartsWith(SearchTb.Text) ||
                    s.NumberPhoneStaff.StartsWith(SearchTb.Text) ||
                    s.Gender.NameGender.StartsWith(SearchTb.Text)).ToList();
            }
            catch (Exception ex)
            {
                MBClass.ErrorMB(ex);
            }
        }
    }
}
