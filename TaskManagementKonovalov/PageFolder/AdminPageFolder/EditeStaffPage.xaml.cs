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
using System.Windows.Threading;

namespace TaskManagementKonovalov.PageFolder.AdminPageFolder
{
    /// <summary>
    /// Логика взаимодействия для EditeStaffPage.xaml
    /// </summary>
    public partial class EditeStaffPage : Page
    {
        Staff staffs = new Staff();

        public EditeStaffPage(Staff staff)
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

            staffs = DBEntities.GetContext().Staff.FirstOrDefault
                (c => c.IdStaff == staff.IdStaff);

            LastNameStaffTb.Text = staffs.LastNameStaff;
            FirstNameStaffTb.Text = staffs.FirstNameStaff;
            MiddleNameStaffTb.Text = staffs.MiddleNameStaff;
            NumberPhoneStaffTb.Text = staffs.NumberPhoneStaff;
            DateOfBirthStaffDp.Text = staffs.DateOfBirthStaff.ToString();
            AdressStaffTb.Text = staffs.AdressStaff;
            PhotoIM.Source = ClassImage.ConvertByteArrayToImage(staffs.PhotoStaff);

            var timer = new DispatcherTimer
            { Interval = TimeSpan.FromSeconds(0.01) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                GenderCb.SelectedValue = staffs.IdGender;
                JobTitleCb.SelectedValue = staffs.IdJobTitle;
                RoleCb.SelectedValue = staffs.User.IdRole;
            };
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

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
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
                staffs.User.IdRole = Int32.Parse(RoleCb.SelectedValue.ToString());

                staffs.LastNameStaff = LastNameStaffTb.Text;
                staffs.FirstNameStaff = FirstNameStaffTb.Text;

                if (string.IsNullOrWhiteSpace(MiddleNameStaffTb.Text)) { }
                else
                {
                    staffs.MiddleNameStaff = MiddleNameStaffTb.Text;
                }

                staffs.NumberPhoneStaff = NumberPhoneStaffTb.Text;
                staffs.DateOfBirthStaff = DateTime.Parse(DateOfBirthStaffDp.Text);
                staffs.AdressStaff = AdressStaffTb.Text;
                staffs.IdGender = Int32.Parse(GenderCb.SelectedValue.ToString());
                staffs.IdJobTitle = Int32.Parse(JobTitleCb.SelectedValue.ToString());
                staffs.User.IdRole = Int32.Parse(RoleCb.SelectedValue.ToString());

                if (selectedFileName != "")
                {
                    staffs.PhotoStaff = ClassImage.ConvertImageToArray(selectedFileName);
                }
                else if (PhotoIM.Source == null)
                {
                    staffs.PhotoStaff = null;
                }
                DBEntities.GetContext().SaveChanges();

                MBClass.InfoMB("Данные о сотруднике были успешно сохранены");
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
