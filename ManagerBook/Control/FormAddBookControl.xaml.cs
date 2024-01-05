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
using static ManagerBook.Control.BookControl;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for FormAddBookControl.xaml
    /// </summary>
    public partial class FormAddBookControl : UserControl
    {
        private BookControl mainBookControl;
        public FormAddBookControl()
        {
            InitializeComponent();
        }

        public FormAddBookControl(BookControl bookControl)
        {
            InitializeComponent();
            mainBookControl = bookControl;
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

        public void AddData(ulong isbn, string title, string des, string price)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "INSERT INTO Books (ISBN, Title, Description, Price) VALUES (@ISBN, @Title, @Description, @Price)";

                // Add parameters with values
                insertCommand.Parameters.AddWithValue("@ISBN", isbn);
                insertCommand.Parameters.AddWithValue("@Title", title);
                insertCommand.Parameters.AddWithValue("@Description", des);
                insertCommand.Parameters.AddWithValue("@Price", price);
                insertCommand.ExecuteReader();
                db.Close();

            }
        }

        private void AddDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtISBN.Text) || string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDes.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลในทุกช่อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!ulong.TryParse(txtISBN.Text, out ulong isbn) || txtISBN.Text.Length != 10)
            {
                MessageBox.Show("รหัสไม่ถูกต้อง. กรุณาใส่รหัสที่เป็นตัวเลขที่ถูกต้องและมี 10 หลัก.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string title = txtTitle.Text;
            string des = txtDes.Text;
            string price = txtPrice.Text;

            AddData(isbn, title, des, price);

            // ล้างฟอร์มหลังจากการเพิ่มข้อมูล
            txtISBN.Clear();
            txtTitle.Clear();
            txtDes.Clear();
            txtPrice.Clear();


            NameBook addedBook = new NameBook
            {
                ISBN = isbn.ToString(),
                Title = title,
                Description = des,
                Price = price
            };

            // อัปเดตข้อมูลใน DataGrid
            mainBookControl.namebooks.Add(addedBook);

        }
    }
}
