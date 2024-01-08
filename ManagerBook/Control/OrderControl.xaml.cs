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
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {

        public ObservableCollection<NameBook> namebooks;
        public ObservableCollection<NameBook> selectedBooks;

        public OrderControl()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            namebooks = new ObservableCollection<NameBook>(GetBooks());
            orderDataGrid.ItemsSource = namebooks;

            selectedBooks = new ObservableCollection<NameBook>();
            listDataGrid.ItemsSource = selectedBooks;

            listDataGrid.SelectionChanged += ListDataGrid_SelectionChanged;

        }

        public List<NameBook> GetSelectedBooks()
        {
            return selectedBooks.ToList();
        }

        public class NameBook
        {
            public string Character { get; set; }
            public Brush BgColor { get; set; }
            public string ISBN { get; set; }
            public string Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public string Phone { get; set; }
            public int Many { get; set; }
            public double TotalPrice { get; set; }

            public NameBook()
            {
                Many = 1;
                TotalPrice = Price;
            }
        }

        public class ReportData
        {
            public string Isbn { get; set; }
            public string BookName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public int TotalPrice { get; internal set; }

        }
 
        //ค้นหา-----------//
        private void Search()
        {
            string searchTerm = textBoxFilter.Text;

            if (string.IsNullOrEmpty(searchTerm))
            {
                // membersDataGrid.ItemsSource = members;
                orderDataGrid.ItemsSource = namebooks;
            }
            else
            {
                var filteredMembers = namebooks.Where(m =>
                    m.ISBN.Contains(searchTerm)
                ).ToList();

                orderDataGrid.ItemsSource = filteredMembers;
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

        //ค้นหา============//

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
                            Price = double.Parse(reader["Price"].ToString())
                        };

                        namebooks.Add(namebook);
                        counter++;
                    }
                }
            }

            return namebooks;
        }


        public List<ReportData> GetReportData()
        {
            List<ReportData> reportData = new List<ReportData>();

            foreach (NameBook book in selectedBooks)
            {
                reportData.Add(new ReportData
                {
                    Isbn = book.ISBN,
                    BookName = book.Title,
                    Quantity = book.Many,
                    Price = book.Price
                });
            }

            return reportData;
        }

        private void gridBasketButton(object sender, RoutedEventArgs e)
        {
            NameBook selectedBook = (NameBook)orderDataGrid.SelectedItem;

            if (selectedBook != null)
            {
                NameBook existingBook = selectedBooks.FirstOrDefault(book => book.ISBN == selectedBook.ISBN);

                if (existingBook != null)
                {
                    existingBook.Many++;
                    existingBook.Price = Convert.ToDouble(existingBook.Price) + Convert.ToDouble(selectedBook.Price);
                    existingBook.TotalPrice += selectedBook.Price;
                    ListDataGrid_SelectionChanged(null, null);
                    UpdateTotalPrice();
                }
                else
                {
                    selectedBooks.Add(new NameBook
                    {
                        ISBN = selectedBook.ISBN,
                        Title = selectedBook.Title,
                        Many = 1,
                        Price = selectedBook.Price,
                        TotalPrice = selectedBook.Price
                    });

                    UpdateTotalPrice();
                    SubmitButton.IsEnabled = true;
                }
            }
        }

        private void DeleteBookButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;

            if (deleteButton != null)
            {
                NameBook selectedBook = deleteButton.DataContext as NameBook;

                if (selectedBook != null)
                {
                    selectedBook.Many--;
                    selectedBook.Price -= selectedBook.Price / (selectedBook.Many + 1);
                    selectedBook.TotalPrice -= selectedBook.Price / (selectedBook.Many);

                    if (selectedBook.Many == 0)
                    {
                        selectedBooks.Remove(selectedBook);
                    }

                    UpdateTotalPrice();
                    ListDataGrid_SelectionChanged(null, null);

                    // Enable or disable the SubmitButton based on whether there are items in selectedBooks
                    SubmitButton.IsEnabled = selectedBooks.Count > 0;
                }
            }
        }



        private void UpdateTotalPrice()
        {
            double totalPrice = selectedBooks.Sum(book => book.TotalPrice);
            totalDataGrid.ItemsSource = new List<NameBook> { new NameBook { Title = "Total Price", TotalPrice = totalPrice } };
        }

        private void ListDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // อัพเดต Data ด้านขวามือ Refresh เมื่อคลิ๊กอีกครั้ง
            listDataGrid.Items.Refresh();
            //UpdateTotalPrice();

        }

        private void totalDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool itemsSelected = totalDataGrid.SelectedItems.Count > 1;
            SubmitButton.IsEnabled = itemsSelected;

        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedBooks.Count == 0)
            {
                MessageBox.Show("กรุณาเลือกหนังสือที่ต้องการสั่งซื้อ", "คำเตือน", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            // Show InsertId before FormInsertCustomerId
            InsertId insertId = new InsertId(GetReportData(), selectedBooks.Sum(book => book.TotalPrice));
            insertId.ShowDialog();

            // ตรวจสอบว่า FormInsertCustomerId ถูกปิดลงหรือไม่
            if (insertId.DialogResult.HasValue && insertId.DialogResult.Value)
            {
                // Show FormInsertCustomerId with SelectedBooks and TotalPrice
                SumReport sumReport = new SumReport(GetReportData(), selectedBooks.Sum(book => book.TotalPrice), insertId.CustomerId, insertId.CustomerName);
                sumReport.ShowDialog();


            }
        }

    }
}
