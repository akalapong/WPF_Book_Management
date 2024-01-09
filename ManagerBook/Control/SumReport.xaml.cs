using ManagerBook.Control;
using Microsoft.Data.Sqlite;
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
using static ManagerBook.Control.CustomerControl;
using System.Xml.Linq;
using static ManagerBook.Control.OrderControl;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for AddCustimer.xaml
    /// </summary>
    public partial class SumReport : Window
    {
       
        public List<ReportData> SelectedBooks { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public SumReport(List<ReportData> selectedBooks, double totalPrice, double totalQuantity, int customerId, string customerName)
        {
            InitializeComponent();
            SelectedBooks = selectedBooks;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
            mainContent.DataContext = this; 
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

        public void AddData(uint isbn, uint customerid, uint totalquatity, uint totalprice)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO Transactions (ISBN, Customer_Id, Quatity, Total_Price) " +
                    "VALUES (@Isbn, @Customer_id, @Quatity, @Total_price)";

                // Add parameters with values
                insertCommand.Parameters.AddWithValue("@Isbn", isbn);
                insertCommand.Parameters.AddWithValue("@Customer_id", customerid);
                insertCommand.Parameters.AddWithValue("@Quatity", totalquatity);
                insertCommand.Parameters.AddWithValue("@Total_price", totalprice);
                insertCommand.ExecuteReader();
                db.Close();

            }
        }

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint isbn = 777777;
                uint customerId = (uint)CustomerId;
                uint totalquatity = (uint)TotalQuantity;
                uint totalPrice = (uint)TotalPrice; 

                
                AddData(isbn, customerId, totalquatity, totalPrice);
                MessageBox.Show("เพิ่มข้อมูลลงในฐานข้อมูลเรียบร้อยแล้ว.");
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"เกิดข้อผิดพลาดในการเพิ่มข้อมูลลงในฐานข้อมูล: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
