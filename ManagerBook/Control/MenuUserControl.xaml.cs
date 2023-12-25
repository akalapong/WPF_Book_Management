using ManagerBook.Views;
using MySql.Data.MySqlClient;
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

namespace ManagerBook.Control
{
    /// <summary>
    /// Interaction logic for MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        private Button _selectedButton;

        // Other code...

        public MenuUserControl()
        {
            InitializeComponent();
            Loaded += MenuUserControl_Loaded;
        }


        private void btnDBoard_Click(object sender, RoutedEventArgs e)
        {
            BoardControl board = new BoardControl();
            Main mainWindow = Window.GetWindow(this) as Main;
            if(mainWindow != null)
            {
                mainWindow.ChangeContent(board);
            }
            SelectButton(btnDBoard);
        }

        private void btnCtm_Click(object sender, RoutedEventArgs e)
        {
            CustomerControl customerControl = new CustomerControl();
            Main mainWindow = Window.GetWindow(this) as Main;
            if (mainWindow != null)
            {
                mainWindow.ChangeContent(customerControl);
            }
            SelectButton(btnCtm);
        }

        private void btnBookMana_Click(object sender, RoutedEventArgs e)
        {
            BookControl bookControl = new BookControl();
            Main mainWindow = Window.GetWindow(this) as Main;
            if (mainWindow != null)
            {
                mainWindow.ChangeContent(bookControl);
            }
            SelectButton(btnBookMana);
        }

        private void btnOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderControl orderControl = new OrderControl();
            Main mainWindow = Window.GetWindow(this) as Main;
            if (mainWindow != null)
            {
                mainWindow.ChangeContent(orderControl);
            }
            SelectButton(btnOrder);
        }

        private void SelectButton(Button button)
        {
            // Update visual state for the previously selected button
            if (_selectedButton != null)
            {
                _selectedButton.Background = Brushes.Transparent; // Set the desired background
            }

            // Update visual state for the newly selected button
            _selectedButton = button;
            if (_selectedButton != null)
            {
                _selectedButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008ae6"));
            }
        }

        private void MenuUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize the selected button to "Dashboard" when the control is loaded
            SelectButton(btnDBoard);
        }
    }
}
