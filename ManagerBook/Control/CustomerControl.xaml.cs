using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using static System.Net.Mime.MediaTypeNames;

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class CustomerControl : UserControl
    {
        ObservableCollection<Member> members = new ObservableCollection<Member>();

        public CustomerControl()
        {
            InitializeComponent();

            var converter = new BrushConverter();
            members = new ObservableCollection<Member>(GetMembers());

            membersDataGrid.ItemsSource = members;
        }

        public class Member
        {
            public string Character { get; set; }
            public Brush BgColor { get; set; }
            public string Num { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        public static List<Member> GetMembers()
        {
            List<Member> members = new List<Member>();

            using (SqliteConnection connection = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                connection.Open();

                string query = "SELECT * FROM Customers";
                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    int counter = 1; // ตัวแปรนับ
                    while (reader.Read())
                    {
                        Member member = new Member
                        {
                            Id = counter.ToString(),
                            Num = reader["Customer_Id"].ToString(),
                            Name = reader["Customers_Name"].ToString(),
                            Address = reader["Address"].ToString(),
                            Email = reader["Email"].ToString()
                            
                        };

                        members.Add(member);
                        counter++;
                    }
                }
            }

            return members;
        }



        private void Search()
        {
            string searchTerm = textBoxFilter.Text;

            if (string.IsNullOrEmpty(searchTerm))
            {
                membersDataGrid.ItemsSource = members;
            }
            else
            {
                var filteredMembers = members.Where(m =>
                    m.Name.Contains(searchTerm) ||
                    m.Num.Contains(searchTerm) ||
                    m.Address.Contains(searchTerm) ||
                    m.Email.Contains(searchTerm)
                ).ToList();

                membersDataGrid.ItemsSource = filteredMembers;
            }
        }


        private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void TextBoxFilter_GotFocus(object sender, RoutedEventArgs e)
        {
            // เมื่อ TextBox ได้รับโฟกัส, ซ่อน TextBlock
            txtSearch.Visibility = Visibility.Collapsed;
        }

        private void TextBoxFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            // เมื่อ TextBox หายโฟกัส, แสดง TextBlock ถ้า TextBox ว่าง
            if (string.IsNullOrEmpty(textBoxFilter.Text))
            {
                txtSearch.Visibility = Visibility.Visible;
            }
        }
    }
}
