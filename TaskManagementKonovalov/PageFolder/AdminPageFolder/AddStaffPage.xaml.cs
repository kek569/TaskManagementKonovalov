using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using Microsoft.Win32;
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
    /// Логика взаимодействия для AddStaffPage.xaml
    /// </summary>
    public partial class AddStaffPage : System.Windows.Controls.Page
    {
        public AddStaffPage()
        {
            path = Directory.GetCurrentDirectory();
            path = path.Remove(path.Length - 9);
            string pathDictionary = (App.Current as App).PathDictionary;

            if (pathDictionary != null && pathDictionary != "")
            {
                this.Resources = new ResourceDictionary() { Source = new Uri(pathDictionary) };
            }
            InitializeComponent();

            GenderCb.ItemsSource = DBEntities.GetContext().Gender.ToList();
            JobTitleCb.ItemsSource = DBEntities.GetContext().JobTitle.ToList();
            RoleCb.ItemsSource = DBEntities.GetContext().Role.ToList();
            NumberPhoneStaffTb.MaxLength = 28;
            DateOfBirthStaffDp.DisplayDateEnd = DateTime.Now.AddYears(-18);
        }

        private string path;

        private void LoadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhoto();
        }

        Staff staff = new Staff();
        string selectedFileName = "";
        private string leng;
        private string TextPassword;
        private string MiddleName;
        private string MiddleNameFull;
        private int Check = 0;
        private int LastIdUser;
        private int LastIdSecretary;
        private int LastIdDirector;
        private int LastIdStaff;
        private int Rem = 0;

        private void AddPhoto()
        {
            try
            {

                OpenFileDialog op = new OpenFileDialog();
                op.InitialDirectory = "";
                op.Filter = "All support graphics|*.jpg;*.jpeg;*.png|" +
                    "JPEG (*.jpg;*jpeg)|*.jpg;*jpeg|" +
                    "Portable Network Graphic (*.png|*.png";

                if (op.ShowDialog() == true)
                {
                    selectedFileName = op.FileName;
                    staff.PhotoStaff = ClassImage.ConvertImageToArray(selectedFileName);
                    PhotoIM.Source = ClassImage.ConvertByteArrayToImage(staff.PhotoStaff);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void AddStaffBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LastNameStaffTb.Text))
            {
                MBClass.ErrorMB("Введите имя");
                LastNameStaffTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(FirstNameStaffTb.Text))
            {
                MBClass.ErrorMB("Введите фамилию");
                FirstNameStaffTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(NumberPhoneStaffTb.Text))
            {
                MBClass.ErrorMB("Введите номер телефона");
                NumberPhoneStaffTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(DateOfBirthStaffDp.Text))
            {
                MBClass.ErrorMB("Введите дату рождения");
                DateOfBirthStaffDp.Focus();
            }
            else if (string.IsNullOrWhiteSpace(AdressStaffTb.Text))
            {
                MBClass.ErrorMB("Введите адрес");
                AdressStaffTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(LoginTb.Text))
            {
                MBClass.ErrorMB("Введите логин");
                LoginTb.Focus();
            }
            else if (string.IsNullOrWhiteSpace(PasswordTb.Text))
            {
                MBClass.ErrorMB("Введите пароль");
                PasswordTb.Focus();
            }
            else if (GenderCb.SelectedIndex <= -1)
            {
                MBClass.ErrorMB("Введите пол");
                GenderCb.Focus();
            }
            else if (JobTitleCb.SelectedIndex <= -1)
            {
                MBClass.ErrorMB("Введите должность");
                JobTitleCb.Focus();
            }
            else if (RoleCb.SelectedIndex <= -1)
            {
                MBClass.ErrorMB("Введите роль");
                RoleCb.Focus();
            }
            else
            {
                Staff staffs = new Staff();
                staffs = DBEntities.GetContext().Staff.FirstOrDefault
                    (s => s.User.LoginUser == LoginTb.Text);

                if (string.IsNullOrWhiteSpace(MiddleNameStaffTb.Text))
                {
                    MiddleName = "";
                    MiddleNameFull = "";
                }
                else
                {
                    MiddleName = MiddleNameStaffTb.Text;
                    MiddleNameFull = MiddleNameStaffTb.Text;
                }

                if (staffs != null)
                {
                    if (staffs.User.LoginUser == LoginTb.Text)
                    {
                        MBClass.ErrorMB("Данный логин уже существует");
                        LoginTb.Focus();
                    }
                    else
                    {
                        MBClass.ErrorMB("Данный логин уже существует");
                        AddStaff();
                    }
                }
                else
                {
                    AddStaff();
                }
            }
        }

        private void AddStaff()
        {
            if (selectedFileName == "")
            {
                var userAdd = new User()
                {
                    LoginUser = LoginTb.Text,
                    PasswordUser = PasswordTb.Text,
                    IdRole = Int32.Parse(RoleCb.SelectedValue.ToString())
                };
                DBEntities.GetContext().User.Add(userAdd);
                DBEntities.GetContext().SaveChanges();

                using (DBEntities db = new DBEntities())
                {
                    LastIdUser = db.User.Max(a => a.IdUser);
                }

                var staffAdd = new Staff()
                {
                    LastNameStaff = LastNameStaffTb.Text,
                    FirstNameStaff = FirstNameStaffTb.Text,
                    MiddleNameStaff = MiddleName,
                    NumberPhoneStaff = NumberPhoneStaffTb.Text,
                    DateOfBirthStaff = DateTime.Parse(DateOfBirthStaffDp.Text),
                    IdGender = Int32.Parse(GenderCb.SelectedValue.ToString()),
                    IdUser = LastIdUser,
                    AdressStaff = AdressStaffTb.Text,
                    IdJobTitle = Int32.Parse(JobTitleCb.SelectedValue.ToString())
                };
                DBEntities.GetContext().Staff.Add(staffAdd);
                DBEntities.GetContext().SaveChanges();

                MBClass.InfoMB("Данные о сотруднике успешно добавлены");
                NavigationService.Navigate(new ListStaffPage());
            }
            else
            {
                var userAdd = new User()
                {
                    LoginUser = LoginTb.Text,
                    PasswordUser = PasswordTb.Text,
                    IdRole = Int32.Parse(RoleCb.SelectedValue.ToString())
                };
                DBEntities.GetContext().User.Add(userAdd);
                DBEntities.GetContext().SaveChanges();

                using (DBEntities db = new DBEntities())
                {
                    LastIdUser = db.User.Max(a => a.IdUser);
                }

                var staffAdd = new Staff()
                {
                    LastNameStaff = LastNameStaffTb.Text,
                    FirstNameStaff = FirstNameStaffTb.Text,
                    MiddleNameStaff = MiddleName,
                    NumberPhoneStaff = NumberPhoneStaffTb.Text,
                    DateOfBirthStaff = System.DateTime.Parse(DateOfBirthStaffDp.Text),
                    IdGender = Int32.Parse(GenderCb.SelectedValue.ToString()),
                    IdUser = LastIdUser,
                    AdressStaff = AdressStaffTb.Text,
                    IdJobTitle = Int32.Parse(JobTitleCb.SelectedValue.ToString()),
                    PhotoStaff = ClassImage.ConvertImageToArray(selectedFileName)
                };
                DBEntities.GetContext().Staff.Add(staffAdd);
                DBEntities.GetContext().SaveChanges();

                MBClass.InfoMB("Данные о сотруднике успешно добавлены");
                NavigationService.Navigate(new ListStaffPage());
            }
        }

        private void GenderCb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void JobTitleCb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void RoleCb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void DateOfBirthStaffDp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void NumberPhoneStaffTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+()-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PhotoIM_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PhotoIM.Source = null;
            selectedFileName = "";
        }
    }
}
