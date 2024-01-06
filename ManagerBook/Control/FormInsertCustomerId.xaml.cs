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
using static ManagerBook.Control.CustomerControl;
using static ManagerBook.Control.FormReport;
using static ManagerBook.Control.OrderControl;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for FormAddCustomerControl.xaml
    /// </summary>
    public partial class FormInsertCustomerId : UserControl
    {

        public FormInsertCustomerId()
        {
            InitializeComponent();
        }

        public List<OrderControl.ReportData> SelectedBooks
        {
            get { return (List<OrderControl.ReportData>)GetValue(SelectedBooksProperty); }
            set { SetValue(SelectedBooksProperty, value); }
        }

        public static readonly DependencyProperty SelectedBooksProperty =
            DependencyProperty.Register("SelectedBooks", typeof(List<OrderControl.ReportData>), typeof(FormInsertCustomerId));

        public double TotalPrice
        {
            get { return (double)GetValue(TotalPriceProperty); }
            set { SetValue(TotalPriceProperty, value); }
        }

        public static readonly DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(double), typeof(FormInsertCustomerId));

        public class CustomerData
        {
            public int isbn { get; set; }
            public int CustomerId { get; set; }
            public int Quatity { get; set; }
            public int TotalPrice { get; set; }
            public string CustomerName { get; set; }
            public string CustomerEmail { get; set; }

        }

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

        public void FindData(uint id)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand findCommand = new SqliteCommand();
                findCommand.Connection = db;

                findCommand.CommandText = "SELECT * FROM Customers WHERE Customer_Id = @Id";
                findCommand.Parameters.AddWithValue("@Id", id);

                SqliteDataReader reader = findCommand.ExecuteReader();

                if (reader.Read())
                {
                    int customerId = reader.GetInt32(0);
                    string customerName = reader["Customers_Name"].ToString();

                    MessageBox.Show($"รหัสลูกค้า!\nID: {customerId}\nคุณ: {customerName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    CustomerData customerData = new CustomerData
                    {
                        CustomerId = customerId,
                        CustomerName = customerName
                    };

                    // Get the selected books data
                    OrderControl orderControlInstance = new OrderControl(); // สร้างอ็อบเจกต์ของ OrderControl
                    List<OrderControl.ReportData> selectedBooksData = orderControlInstance.GetReportData();

                    // Show the Report form and close the FormInsertCustomerId form
                    Report report = new Report(selectedBooksData);
                    report.DataContext = customerData;
                    report.ShowDialog();

                    CloseFormInsertCustomerId();
                }
                else
                {
                    MessageBox.Show($"ไม่พบ. รหัสลูกค้า {id} ", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                reader.Close();
                db.Close();
            }
        }
        private void CloseFormInsertCustomerId()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        public void FindCustomerButton_Click(object sender, RoutedEventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txtId.Text))
                {
                    MessageBox.Show("กรุณากรอกข้อมูลในทุกช่อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!uint.TryParse(txtId.Text, out uint id) || txtId.Text.Length != 5)
                {
                    MessageBox.Show("รหัสไม่ถูกต้อง. กรุณาใส่รหัสที่เป็นตัวเลขที่ถูกต้องและมี 5 หลัก.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                FindData(id);
            }



        }
}
