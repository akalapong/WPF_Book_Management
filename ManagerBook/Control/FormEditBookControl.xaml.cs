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
using static ManagerBook.Control.BookControl;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for FormAddCustomerControl.xaml
    /// </summary>
    public partial class FormEditBookControl : UserControl
    {

        private BookControl mainbookControl;
        public FormEditBookControl()
        {
            InitializeComponent();
        }
        public FormEditBookControl(BookControl bookControl, NameBook selectedBook)
        {
            InitializeComponent();
            mainbookControl = bookControl;
            LoadBookData(selectedBook);

        }


        //ดึงข้อมู,จากหน้า CustomerControl
        public void LoadBookData(NameBook selectedBook)
        {
            // Display member data in the form
            Isbn.Text = selectedBook.ISBN;
            Title.Text = selectedBook.Title;
            Description.Text = selectedBook.Description;
            Price.Text = selectedBook.Price;
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

        public void EditData(string isbn, string title, string description, string price)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();
                using (SqliteCommand updateCommand = new SqliteCommand())
                {
                    updateCommand.Connection = db;

                    // Updated UPDATE command
                    updateCommand.CommandText = "UPDATE Books SET Title = @Title, Description = @Description, Price = @Price WHERE ISBN = @ISBN";

                    // Add parameters with values
                    updateCommand.Parameters.AddWithValue("@ISBN", isbn);
                    updateCommand.Parameters.AddWithValue("@Title", title);
                    updateCommand.Parameters.AddWithValue("@Description", description);
                    updateCommand.Parameters.AddWithValue("@Price", price);

                    // Execute the command
                    updateCommand.ExecuteNonQuery();
                }
                db.Close();
            }
        }


        private void EditDataButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Isbn.Text) || string.IsNullOrWhiteSpace(Title.Text) ||
                string.IsNullOrWhiteSpace(Description.Text) || string.IsNullOrWhiteSpace(Price.Text))
            {
                MessageBox.Show("กรุณากรอกข้อมูลในทุกช่อง.", "ข้อผิดพลาด", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string isbn = Isbn.Text.Trim();
            string title = Title.Text;
            string description = Description.Text;
            string price = Price.Text;

            // แก้ไขข้อมูลในฐานข้อมูล
            EditData(isbn, title, description, price);

            // อัปเดตข้อมูลใน ObservableCollection
            NameBook editBook = mainbookControl.namebooks.FirstOrDefault(m => m.ISBN == isbn.ToString());
            if (editBook != null)
            {
                editBook.Title = title;
                editBook.Description = description;
                editBook.Price = price;
            }

            // อัปเดตข้อมูลใน DataGrid
            mainbookControl.booksDataGrid.ItemsSource = null;
            mainbookControl.booksDataGrid.ItemsSource = mainbookControl.namebooks;

            Window window = Window.GetWindow(this);

            if (window != null)
            {
                window.Close();
            }
        }
    }
}