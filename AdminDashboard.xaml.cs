using System.Windows;
using System.Windows.Controls;

namespace MovieTicket_G3
{
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }

        private void btnMovieUpdate_Click(object sender, RoutedEventArgs e)
        {
            AddButton.Visibility = Visibility.Visible;
            DelButton.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Visible;
        }

        private void btnSnackInventory_Click(object sender, RoutedEventArgs e)
        {
            AddSnack.Visibility = Visibility.Visible;
            DelSnack.Visibility = Visibility.Visible;
            EditSnack.Visibility = Visibility.Visible;
        }

        private void btnBookingMenu_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSnack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DelSnack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditSnack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Movie1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Movie2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Movie3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Movie4_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}