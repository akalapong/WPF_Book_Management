using ManagerBook.Views;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static ManagerBook.Control.CustomerControl;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for BookControl.xaml
    /// </summary>
    public partial class BookControl : UserControl
    {
        public DataGrid BooksDataGrid
        {
            get { return booksDataGrid; }
        }
        public ObservableCollection<NameBook> namebooks;
        public BookControl()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            namebooks = new ObservableCollection<NameBook>(GetBooks());
            booksDataGrid.ItemsSource = namebooks;
        }

        public class NameBook
        {
            public string Character { get; set; }
            public Brush BgColor { get; set; }
            public string ISBN { get; set; }
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public string Phone { get; set; }
        }

        public static List<NameBook> GetBooks()
        {
            List<NameBook> namebooks = new List<NameBook>();

            using (SqliteConnection connection = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Books";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    int counter = 1; // ตัวแปรนับ
                    while (reader.Read())
                    {
                        NameBook namebook = new NameBook
                        {
                            Id = counter.ToString(),
                            ISBN = reader["ISBN"].ToString(),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = reader["Price"].ToString()

                        };

                        namebooks.Add(namebook);
                        counter++;
                    }
                }
            }

            return namebooks;
        }

        private void Search()
        {
            string searchTerm = textBoxFilter.Text;

            if (string.IsNullOrEmpty(searchTerm))
            {
                // membersDataGrid.ItemsSource = members;
                booksDataGrid.ItemsSource = namebooks;
            }
            else
            {
                var filteredMembers = namebooks.Where(m =>
                    m.ISBN.Contains(searchTerm) ||
                    m.Title.Contains(searchTerm) ||
                    m.Description.Contains(searchTerm) ||
                    m.Price.Contains(searchTerm)
                ).ToList();

                booksDataGrid.ItemsSource = filteredMembers;
            }
        }

        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void TextBoxFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            txtSearch.Visibility = Visibility.Collapsed;
        }

        private void TextBoxFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFilter.Text))
            {
                txtSearch.Visibility = Visibility.Visible;
            }
        }


        private void DeleteBook(string isbnID)
        {
            using (SqliteConnection connection = new SqliteConnection("Filename=sqliteSample.db"))
            {
                connection.Open();

                // คำสั่ง SQL เพื่อลบข้อมูล
                string query = "DELETE FROM Books WHERE ISBN = @ISBN";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    // กำหนดค่าพารามิเตอร์
                    command.Parameters.AddWithValue("@ISBN", isbnID);

                    // ทำการ execute คำสั่ง SQL
                    command.ExecuteNonQuery();
                }
            }
        }
        private void EditBookButton_Click(object sender, RoutedEventArgs e)
        {

            NameBook selectedBook = (NameBook)booksDataGrid.SelectedItem;

            if (selectedBook != null)
            {
                // สร้าง FormEditCustomerControl และส่งข้อมูลไปยังโดยใช้คอนสตรักเตอร์ที่มีพารามิเตอร์ Member
                //FormEditCustomerControl editForm = new FormEditCustomerControl(selectedMember);

                EditBook editBook = new EditBook(this, selectedBook);//this เข้าไปเพื่อส่งตัวอ็อบเจ็กต์ CustomerControl ไปยัง EditCustomer.
                editBook.ShowDialog();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกลูกค้าที่ต้องการแก้ไข.");
            }

            //EditCustomer editCustomer = new EditCustomer();
            //editCustomer.Show();

        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            // รับข้อมูลที่เลือกจาก DataGrid หรือให้ค่าตามที่คุณต้องการ
            NameBook selectedBook = (NameBook)booksDataGrid.SelectedItem;
            //Member selectedMember = (Member)membersDataGrid.SelectedItem;

            if (selectedBook != null)
            {
                MessageBoxResult result = MessageBox.Show("คุณต้องการจะลบใช่ไหม?", "ยืนยันการลบ", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteBook(selectedBook.ISBN);
                    namebooks.Remove(selectedBook);
                    booksDataGrid.ItemsSource = null;
                    booksDataGrid.ItemsSource = namebooks;


                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }

        private void AddNewBookButton_Click(object sender, RoutedEventArgs e)
        {

            AddBook addBook = new AddBook(this);
            addBook.ShowDialog();
        }
    }
}
