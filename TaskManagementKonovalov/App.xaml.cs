using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

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
    }
}
