using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.WindowFolder;
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
    /// Логика взаимодействия для AddPassportPage.xaml
    /// </summary>
    public partial class AddPassportPage : Page
    {
        public AddPassportPage()
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            PassportSeriesTb.MaxLength = 4;
            PassportNumberTb.MaxLength = 6;

            PassportSeriesTb.Text = (App.Current as App).PassportSeriesTextAdd;
            PassportNumberTb.Text = (App.Current as App).PassportNumberTextAdd;
            PlaceIssueTb.Text = (App.Current as App).PlaceIssueTextAdd;
            DateUssuePb.Text = (App.Current as App).DateUssueTextAdd;
            DepartmentCodeTb.Text = (App.Current as App).DepartmentCodeTextAdd;
            RegistrationAddressTb.Text = (App.Current as App).RegistrationAddressTextAdd;
        }

        private string path;

        private void LessBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PassportSeriesTb.Text))
            {
                Qest("серий паспорта");
            }
            else if (string.IsNullOrWhiteSpace(PassportNumberTb.Text))
            {
                Qest("номер паспорта");
            }
            else if (string.IsNullOrWhiteSpace(PlaceIssueTb.Text))
            {
                Qest("месте выдачи");
            }
            else if (string.IsNullOrWhiteSpace(DateUssuePb.Text))
            {
                Qest("дата выдачи");
            }
            else if (string.IsNullOrWhiteSpace(DepartmentCodeTb.Text))
            {
                Qest("код подразделения");
            }
            else if (string.IsNullOrWhiteSpace(RegistrationAddressTb.Text))
            {
                Qest("адрес регистраций");
            }
            else
            {
                (App.Current as App).PassportSeriesTextAdd = PassportSeriesTb.Text;
                (App.Current as App).PassportNumberTextAdd = PassportNumberTb.Text;
                (App.Current as App).PlaceIssueTextAdd = PlaceIssueTb.Text;
                (App.Current as App).DateUssueTextAdd = DateUssuePb.Text;
                (App.Current as App).DepartmentCodeTextAdd = DepartmentCodeTb.Text;
                (App.Current as App).RegistrationAddressTextAdd = RegistrationAddressTb.Text;

                (App.Current as App).PassportAdd = true;

                NavigationService.Navigate(new AddStaffPage());
            }
        }

        private void Qest(string qest)
        {
            bool ret = MBClass.QestionMB("Вы не велие данные о " + qest + 
                "\nТочно хотите выйти обратно в добавление клиента?");
            if (ret == true)
            {
                (App.Current as App).PassportSeriesTextAdd = PassportSeriesTb.Text;
                (App.Current as App).PassportNumberTextAdd = PassportNumberTb.Text;
                (App.Current as App).PlaceIssueTextAdd = PlaceIssueTb.Text;
                (App.Current as App).DateUssueTextAdd = DateUssuePb.Text;
                (App.Current as App).DepartmentCodeTextAdd = DepartmentCodeTb.Text;
                (App.Current as App).RegistrationAddressTextAdd = RegistrationAddressTb.Text;

                NavigationService.Navigate(new AddStaffPage());
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
