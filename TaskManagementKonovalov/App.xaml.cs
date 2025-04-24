using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TaskManagementKonovalov
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string Path { get; set; }
        public string PathDictionary { get; set; }
        public string ThemeSettings { get; set; }
        public string ThemeNow { get; set; }
        public string DeptName { get; set; }
        public string LastNameStaffTextAdd { get; set; }
        public string FirstNameStaffTextAdd { get; set; }
        public string MiddleNameStaffTextAdd { get; set; }
        public string NumberPhoneStaffTextAdd { get; set; }
        public string DateOfBirthStaffTextAdd { get; set; }
        public string AdressStaffTextAdd { get; set; }
        public string PassportSeriesTextAdd { get; set; }
        public string PassportNumberTextAdd { get; set; }
        public ImageSource PhotoStaffAdd { get; set; }
        public string SelectedFileNameStaffAdd { get; set; }
        public object GenderSelectedValueAdd { get; set; }
        public string PlaceIssueTextAdd { get; set; }
        public string DateUssueTextAdd { get; set; }
        public string DepartmentCodeTextAdd { get; set; }
        public string RegistrationAddressTextAdd { get; set; }
        public bool PassportAdd { get; set; }
    }
}
