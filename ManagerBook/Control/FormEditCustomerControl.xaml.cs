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
    public partial class FormEditCustomerControl : UserControl
    {

        private CustomerControl mainCustomerControl;
        //private Member selectedMember;

        public FormEditCustomerControl()
        {
            InitializeComponent();
        }
        public FormEditCustomerControl(CustomerControl customerControl, Member selectedMember)
        {
            InitializeComponent();
            mainCustomerControl = customerControl;
            LoadMemberData(selectedMember);

        }


        //ดึงข้อมู,จากหน้า CustomerControl
        public void LoadMemberData(Member selectedMember)
        {
            // Display member data in the form
            txtId.Text = selectedMember.Num;
            txtName.Text = selectedMember.Name;
            txtAddress.Text = selectedMember.Address;
            txtEmail.Text = selectedMember.Email;
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

        public void EditData(uint id, string name, string address, string email)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                using (SqliteCommand updateCommand = new SqliteCommand())
                {
                    updateCommand.Connection = db;

                    // Updated UPDATE command
                    updateCommand.CommandText = "UPDATE Customers SET Customers_Name = @Name, Address = @Address, Email = @Email WHERE Customer_Id = @Id";

                    // Add parameters with values
                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@Name", name);
                    updateCommand.Parameters.AddWithValue("@Address", address);
                    updateCommand.Parameters.AddWithValue("@Email", email);

                    // Execute the command
                    updateCommand.ExecuteNonQuery();
                }
                db.Close();
            }
        }


        private void EditDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text) || string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลในทุกช่อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!uint.TryParse(txtId.Text, out uint id))
            {
                MessageBox.Show("รหัสไม่ถูกต้อง. กรุณาใส่รหัสที่เป็นตัวเลขที่ถูกต้อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string name = txtName.Text;
            string address = txtAddress.Text;
            string email = txtEmail.Text;

            // แก้ไขข้อมูลในฐานข้อมูล
            EditData(id, name, address, email);

            // อัปเดตข้อมูลใน ObservableCollection
            Member editedMember = mainCustomerControl.members.FirstOrDefault(m => m.Num == id.ToString());
            if (editedMember != null)
            {
                editedMember.Name = name;
                editedMember.Address = address;
                editedMember.Email = email;
            }

            // อัปเดตข้อมูลใน DataGrid
            mainCustomerControl.membersDataGrid.ItemsSource = null;
            mainCustomerControl.membersDataGrid.ItemsSource = mainCustomerControl.members;

            Window window = Window.GetWindow(this);

            if (window != null)
            {
                window.Close();
            }
        }
    }
}