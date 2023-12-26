using System;
using System.Collections.Generic;
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

namespace ManagerBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataAccesss.GetData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> entries = DataAccesss.GetData();
            if (entries.Count > 0)
            {
                string msg = string.Join("\r\n", entries.Select((entry, index) => $"{index + 1}. {entry}"));

                MessageBox.Show(msg, "แสดงข้อมูล");
            }
            else
            {
                MessageBox.Show("ไม่มีข้อมูล.", "แสดงข้อมูล");
            }
        }
    }
}
