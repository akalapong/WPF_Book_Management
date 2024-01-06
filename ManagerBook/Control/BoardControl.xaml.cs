using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
using LiveCharts;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for BoardControl.xaml
    /// </summary>
    public partial class BoardControl : UserControl, INotifyPropertyChanged
    {
        
        public BoardControl()
        {
            InitializeComponent();
            DataContext = this;
            CheckBooks();
        }


        private int _totalBooks;
        private int _totalCustomers;
        public int TotalBooks
        {
            get { return _totalBooks; }
            set
            {
                if (_totalBooks != value)
                {
                    _totalBooks = value;
                    OnPropertyChanged(nameof(TotalBooks));
                }
            }
        }
        public int TotalCustomers
        {
            get { return _totalCustomers; }
            set
            {
                if (_totalCustomers != value)
                {
                    _totalCustomers = value;
                    OnPropertyChanged(nameof(TotalCustomers));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CheckBooks()
        {
            using (SqliteConnection connection = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                connection.Open();

                
                //string saleBooksQuery = "SELECT COUNT(*) FROM Sales WHERE SaleDate = CURRENT_DATE";

                string booksQuery = "SELECT COUNT(*) FROM Books";
                using (SqliteCommand booksCommand = new SqliteCommand(booksQuery, connection))
                {
                    object booksResult = booksCommand.ExecuteScalar();
                    TotalBooks = (booksResult != null) ? Convert.ToInt32(booksResult) : 0;
                    Console.WriteLine($"Total number of books: {TotalBooks}");
                }

                // Count total customers
                string customerQuery = "SELECT COUNT(*) FROM Customers";
                using (SqliteCommand customerCommand = new SqliteCommand(customerQuery, connection))
                {
                    object customerResult = customerCommand.ExecuteScalar();
                    TotalCustomers = (customerResult != null) ? Convert.ToInt32(customerResult) : 0;
                    Console.WriteLine($"Total number of customers: {TotalCustomers}");
                }
            }

        }
    }
}
