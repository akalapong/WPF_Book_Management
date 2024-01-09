using ManagerBook.Views;
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
using static ManagerBook.Control.OrderControl;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for InsertId.xaml
    /// </summary>
    public partial class InsertId : Window
    {
        public List<ReportData> SelectedBooks { get; set; }
        public double TotalPrice { get; set; }
        public double TotalQuantity { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public InsertId(List<ReportData> selectedBooks, double totalPrice, double totalQuantity)
        {
            InitializeComponent();
            SelectedBooks = selectedBooks;
            TotalPrice = totalPrice;
            TotalQuantity = totalQuantity;
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


        private void CloseFormInsertCustomerId()
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            // ตรวจสอบว่ารหัสลูกค้าที่กรอกมีความยาวเท่ากับ 5 หรือไม่
            if (txtId.Text.Length == 5 && int.TryParse(txtId.Text, out int customerId))
            {
                // ตรวจสอบว่ารหัสลูกค้านี้มีอยู่ในฐานข้อมูลหรือไม่
                if (IsCustomerIdExists(customerId, out string customerName))
                {
                    // ถ้ารหัสลูกค้าถูกต้อง, สร้างหน้า InsertCustomer และส่งรหัสลูกค้าไป
                    CustomerId = customerId;
                    CustomerName = customerName;
                    DialogResult = true;
                    CloseFormInsertCustomerId();
                }
                else
                {
                    MessageBox.Show("ไม่พบข้อมูลลูกค้า", "คำเตือน", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("รหัสลูกค้าต้องมีความยาว 5 ตัวเลข", "คำเตือน", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool IsCustomerIdExists(int customerId, out string customerName) //ส่งรหัสลูกค้าและชื่อ
        {
            customerName = null;

            using (SqliteConnection connection = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                connection.Open();

                // สร้างคำสั่ง SQL สำหรับค้นหารหัสลูกค้าและชื่อลูกค้า
                string query = $"SELECT Customer_Id, Customers_Name FROM Customers WHERE Customer_Id = {customerId}";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // ถ้าพบข้อมูลในฐานข้อมูล
                        customerName = reader["Customers_Name"].ToString();
                        return true;  // คืนค่าเป็น true เพราะรหัสลูกค้าถูกต้อง
                    }
                    else
                    {
                        return false; // ไม่พบข้อมูลในฐานข้อมูล
                    }
                }
            }
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
