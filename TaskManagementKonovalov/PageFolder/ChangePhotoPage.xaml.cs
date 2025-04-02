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
using Microsoft.Win32;
using TaskManagementKonovalov.ClassFolder;
using TaskManagementKonovalov.DataFolder;
using TaskManagementKonovalov.WindowFolder.AdminWindowFolder;
using TaskManagementKonovalov.WindowFolder.StaffWindowFolder;

namespace TaskManagementKonovalov.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для ChangePhotoPage.xaml
    /// </summary>
    public partial class ChangePhotoPage : Page
    {
        Staff staffs = new Staff();

        public ChangePhotoPage(string windowName, System.Windows.Window window)
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

            staffs = DBEntities.GetContext().Staff.FirstOrDefault
            (s => s.User.LoginUser == deptName);

            PhotoIM.Source = ClassImage.ConvertByteArrayToImage(staffs.PhotoStaff);

            windowsName = windowName;
            windows = window;
        }

        private string path;
        private string windowsName;
        private System.Windows.Window windows;

        private void LoadPhotoBtn_Click(object sender, RoutedEventArgs e)
        {
            AddPhoto();
        }

        Staff staff = new Staff();
        string selectedFileName = "";

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
            if (selectedFileName != "")
            {
                staffs.PhotoStaff = ClassImage.ConvertImageToArray(selectedFileName);
            }
            else if (PhotoIM.Source == null)
            {
                staffs.PhotoStaff = null;
            }
            DBEntities.GetContext().SaveChanges();

            MBClass.InfoMB("Данные о фотографии были успешно сохранены");
            Restart();
        }

        private void Restart()
        {
            if (windowsName == "MainWindowStaff")
            {
                new MainWindowStaff(true).Show();
                windows.Close();
            }
            else if (windowsName == "MainWindowAdmin")
            {
                new MainWindowAdmin(true).Show();
                windows.Close();
            }
        }

        private void PhotoIM_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PhotoIM.Source = null;
            selectedFileName = "";
        }
    }
}
