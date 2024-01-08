using ManagerBook.Control;
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

namespace ManagerBook.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();

            txtUser.PreviewKeyDown += txtUser_PreviewKeyDown;
            passwordBox.PreviewKeyDown += passwordBox_PreviewKeyDown;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }


        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password) && passwordBox.Password.Length > 0)
                textPassword.Visibility = Visibility.Collapsed;
            else
                textPassword.Visibility = Visibility.Visible;
        }

        private void textPassword_MouseDown(object sender, MouseButtonEventArgs e)
        {
            passwordBox.Focus();
        }

        

        private void txtUser_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUser.Text) && txtUser.Text.Length > 0)
                textUser.Visibility = Visibility.Collapsed;
            else
                textUser.Visibility = Visibility.Visible;
        }

        private void textUser_MouseDown(object sender, MouseButtonEventArgs e)
        {
            txtUser.Focus();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool CheckUser(string userid, out string passwordch)
        {
            passwordch = null;
            using (SqliteConnection connection = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                connection.Open();

                string query = $"SELECT username, password FROM User WHERE username = '{userid}'";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // ถ้าพบข้อมูลในฐานข้อมูล
                        passwordch = reader["password"].ToString();
                        return true;
                    }
                    else
                    {
                        return false; // ไม่พบข้อมูลในฐานข้อมูล
                    }
                }
            }
        }

        private void txtUser_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // ป้องกันการเพิ่ม newline ใน TextBox
                passwordBox.Focus();
            }
        }

        private void passwordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // ป้องกันการเพิ่ม newline ใน TextBox
                Button_Click(sender, e);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("กรุณาใส่ User และ Password.");
            }
            else
            {
                string userId = txtUser.Text;
                string password = passwordBox.Password;

                if (CheckUser(userId, out string passwordFromDB) && password == passwordFromDB)
                {
                    //MessageBox.Show("เข้าระบบสำเร็จ!");



                    // เปิดหน้า Main
                    Main main = new Main();
                    main.Show();

                    // ปิดหน้า Login
                    this.Close();
                }
                else
                {
                    MessageBox.Show("ข้อมูลผิดพลาด. กรุณาตรวจสอบ User และ Password.");
                }
            }
        }
    }
}
