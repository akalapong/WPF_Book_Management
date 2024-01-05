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

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for FormAddCustomerControl.xaml
    /// </summary>
    public partial class FormAddCustomerControl : UserControl
    {
        private CustomerControl mainCustomerControl;
        public FormAddCustomerControl()
        {
            InitializeComponent();
        }

        public FormAddCustomerControl(CustomerControl customerControl)
        {
            InitializeComponent();
            mainCustomerControl = customerControl;
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

        public void AddData(uint id, string name, string address, string email)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO Customers (Customer_Id, Customers_Name, Address, Email) VALUES (@Id, @Name, @Address, @Email)";

                // Add parameters with values
                insertCommand.Parameters.AddWithValue("@Id", id);
                insertCommand.Parameters.AddWithValue("@Name", name);
                insertCommand.Parameters.AddWithValue("@Address", address);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.ExecuteReader();
                db.Close();

            }
        }




        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลในทุกช่อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!uint.TryParse(txtId.Text, out uint id) || txtId.Text.Length != 5)
            {
                MessageBox.Show("รหัสไม่ถูกต้อง. กรุณาใส่รหัสที่เป็นตัวเลขที่ถูกต้องและมี 5 หลัก.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string name = txtName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;

            AddData(id, name, address, email);

            // ล้างฟอร์มหลังจากการเพิ่มข้อมูล
            txtId.Clear();
            txtName.Clear();
            txtAddress.Clear();
            txtEmail.Clear();


            Member addedMember = new Member
            {
                Num = id.ToString(),
                Name = name,
                Address = address,
                Email = email
            };

            // อัปเดตข้อมูลใน DataGrid
            mainCustomerControl.members.Add(addedMember);

        }

    }
}
