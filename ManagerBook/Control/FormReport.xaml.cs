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
using ManagerBook.Views;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;
using static ManagerBook.Control.OrderControl;
using static ManagerBook.Control.FormInsertCustomerId;
using System.Diagnostics;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for FormAddCustomerControl.xaml
    /// </summary>
    public partial class FormReport : UserControl
    {
        //public FormInsertCustomerId customerData;
        private CustomerData customerData;
        public FormReport()
        {
            InitializeComponent();

        }


        public FormReport(CustomerData data)
        {
            InitializeComponent();
            this.customerData = data;
            DataContext = this.customerData;

            Debug.WriteLine($"CustomerId: {customerData.CustomerId}, CustomerName: {customerData.CustomerName}");
            // หรือ
            MessageBox.Show($"CustomerId: {customerData.CustomerId}, CustomerName: {customerData.CustomerName}");
        }

        public List<OrderControl.ReportData> SelectedBooks
        {
            get { return (List<OrderControl.ReportData>)GetValue(SelectedBooksProperty); }
            set { SetValue(SelectedBooksProperty, value); }
        }

        public static readonly DependencyProperty SelectedBooksProperty =
            DependencyProperty.Register("SelectedBooks", typeof(List<OrderControl.ReportData>), typeof(FormReport));

        public double TotalPrice
        {
            get { return (double)GetValue(TotalPriceProperty); }
            set { SetValue(TotalPriceProperty, value); }
        }

        public static readonly DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(double), typeof(FormReport));
    

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Ignore non-numeric input
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }



        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (customerData != null)
            {
                using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
                {
                    db.Open();
                    InsertTransactionData(customerData);
                }
            }
            else
            {
                MessageBox.Show($"ไม่มีข้อมูล", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InsertTransactionData(CustomerData customerData)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // ใส่คำสั่ง SQL สำหรับ INSERT INTO Transactions
                insertCommand.CommandText = "INSERT INTO Transactions (Customer_Id) VALUES (@CustomerId)";

                // ใส่ค่าพารามิเตอร์ที่จำเป็น
                insertCommand.Parameters.AddWithValue("@CustomerId", customerData.CustomerId);

                // ทำการ execute คำสั่ง SQL
                insertCommand.ExecuteNonQuery();
                db.Close();
            }
        }


    }
}
