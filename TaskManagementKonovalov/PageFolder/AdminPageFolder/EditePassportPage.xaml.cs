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

namespace TaskManagementKonovalov.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для EditePassportPage.xaml
    /// </summary>
    public partial class EditePassportPage : Page
    {
        Staff Staffs = new Staff();

        public EditePassportPage(Staff Staff)
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            Staffs = DBEntities.GetContext().Staff.FirstOrDefault
                (c => c.IdStaff == Staff.IdStaff);

            PassportSeriesTb.Text = Staffs.Passport.SeriesPassport.ToString();
            PassportNumberTb.Text = Staffs.Passport.NumberPassport.ToString();
            PlaceIssueTb.Text = Staffs.Passport.PlaceIssuePassport;
            DateUssuePb.Text = Staffs.Passport.DateUssuePassport.ToString();
            DepartmentCodeTb.Text = Staffs.Passport.DepartmentCodePassport;
            RegistrationAddressTb.Text = Staffs.Passport.RegistrationAddressPassport;
        }

        private string path;

        private void LessBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditeStaffPage(Staffs));
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PassportSeriesTb.Text))
            {
                MBClass.ErrorMB("Введите данные о серий паспорта");
                PassportSeriesTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PassportNumberTb.Text))
            {
                MBClass.ErrorMB("Введите данные о номере паспорта");
                PassportSeriesTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PlaceIssueTb.Text))
            {
                MBClass.ErrorMB("Введите данные о место выдачи");
                PassportSeriesTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(DateUssuePb.Text))
            {
                MBClass.ErrorMB("Введите данные о дата выдачи");
                PassportSeriesTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(DepartmentCodeTb.Text))
            {
                MBClass.ErrorMB("Введите данные о код подразделения");
                PassportSeriesTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(RegistrationAddressTb.Text))
            {
                MBClass.ErrorMB("Введите данные о адрес регистраций");
                PassportSeriesTb.Focus();
            }
            else
            {
                Staffs.Passport.SeriesPassport = Int32.Parse(PassportSeriesTb.Text);
                Staffs.Passport.NumberPassport = Int32.Parse(PassportNumberTb.Text);
                Staffs.Passport.PlaceIssuePassport = PlaceIssueTb.Text;
                Staffs.Passport.DateUssuePassport = DateTime.Parse(DateUssuePb.Text);
                Staffs.Passport.DepartmentCodePassport = DepartmentCodeTb.Text;
                Staffs.Passport.RegistrationAddressPassport = RegistrationAddressTb.Text;
                DBEntities.GetContext().SaveChanges();

                MBClass.InfoMB("Данные о паспортных данных клиенты были успешно сохранены");
                NavigationService.Navigate(new ListStaffPage());
            }
        }

        private void PassportSeriesTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PassportNumberTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DateUssuePb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void DepartmentCodeTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
