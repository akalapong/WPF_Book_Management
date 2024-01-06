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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ManagerBook.Control.OrderControl;

namespace ManagerBook.Views
{
    /// <summary>
    /// Interaction logic for AddCustimer.xaml
    /// </summary>
    public partial class InsertCustomer : Window
    {
       
        public List<ReportData> SelectedBooks { get; set; }
        public double TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public InsertCustomer(List<ReportData> selectedBooks, double totalPrice, int customerId, string customerName)
        {
            InitializeComponent();
            SelectedBooks = selectedBooks;
            TotalPrice = totalPrice;
            mainContent.DataContext = this; // Set DataContext to this for proper binding
            CustomerId = customerId;
            CustomerName = customerName;
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
