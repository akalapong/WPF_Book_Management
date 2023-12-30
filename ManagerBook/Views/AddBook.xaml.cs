using ManagerBook.Control;
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

namespace ManagerBook.Views
{
    /// <summary>
    /// Interaction logic for AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public AddBook(BookControl bookControl)
        {
            InitializeComponent();
            FormAddBookControl addFormBook = new FormAddBookControl(bookControl);
            mainContent.Children.Add(addFormBook);
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);

            if (window != null)
            {
                window.Close();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
