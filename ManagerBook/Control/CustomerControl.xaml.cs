using ManagerBook.Views;
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
        public DataGrid MembersDataGrid
        {
            get { return membersDataGrid; }
        }

        public ObservableCollection<Member> members;


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
            txtSearch.Visibility = Visibility.Collapsed;
        }

        private void TextBoxFilter_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFilter.Text))
            {
                txtSearch.Visibility = Visibility.Visible;
            }
        }


        //Button AddData
        private void AddNewMemberButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddCustomer addCustomer = new AddCustomer(this);
            addCustomer.ShowDialog();
        }



        //Delete Data
        private void DeleteMember(string memberId)
        {
            using (SqliteConnection connection = new SqliteConnection("Filename=sqliteSample.db"))
            {
                connection.Open();

                // คำสั่ง SQL เพื่อลบข้อมูล
                string query = "DELETE FROM Customers WHERE Customer_Id = @MemberId";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                {
                    // กำหนดค่าพารามิเตอร์
                    command.Parameters.AddWithValue("@MemberId", memberId);

                    // ทำการ execute คำสั่ง SQL
                    command.ExecuteNonQuery();
                }
            }
        }

        //public event Action<Member> EditButtonClicked;
        private void EditMemberButton_Click(object sender, RoutedEventArgs e)
        {

            Member selectedMember = (Member)membersDataGrid.SelectedItem;

            if (selectedMember != null)
            {
                // สร้าง FormEditCustomerControl และส่งข้อมูลไปยังโดยใช้คอนสตรักเตอร์ที่มีพารามิเตอร์ Member
                //FormEditCustomerControl editForm = new FormEditCustomerControl(selectedMember);

                EditCustomer editCustomer = new EditCustomer(this,selectedMember);  //this เข้าไปเพื่อส่งตัวอ็อบเจ็กต์ CustomerControl ไปยัง EditCustomer.
                editCustomer.ShowDialog();
            }
            else
            {
                MessageBox.Show("กรุณาเลือกลูกค้าที่ต้องการแก้ไข.");
            }

            //EditCustomer editCustomer = new EditCustomer();
            //editCustomer.Show();

        }

        // เรียกใช้งานเมื่อต้องการลบข้อมูล
        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // รับข้อมูลที่เลือกจาก DataGrid หรือให้ค่าตามที่คุณต้องการ
            Member selectedMember = (Member)membersDataGrid.SelectedItem;

            if (selectedMember != null)
            {
                MessageBoxResult result = MessageBox.Show("คุณต้องการจะลบใช่ไหม?", "ยืนยันการลบ", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    DeleteMember(selectedMember.Num);
                    members.Remove(selectedMember);
                    membersDataGrid.ItemsSource = null;
                    membersDataGrid.ItemsSource = members;

                   
                }
            }
            else
            {
                MessageBox.Show("Please select a member to delete.");
            }
        }



    }
}
