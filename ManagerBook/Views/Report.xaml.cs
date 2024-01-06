using ManagerBook.Control;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ManagerBook.Control.CustomerControl;

namespace ManagerBook.Views
{
    /// <summary>
    /// Interaction logic for AddCustimer.xaml
    /// </summary>
    public partial class Report : Window
    {
        
        public Report()
        {
            InitializeComponent();
        }
        public Report(List<OrderControl.ReportData> data)
        {
            InitializeComponent();
            LoadData(data);
        }

        private void LoadData(List<OrderControl.ReportData> data)
        {
            DataContext = data;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window != null)
            {
                window.Close();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

    }
}
