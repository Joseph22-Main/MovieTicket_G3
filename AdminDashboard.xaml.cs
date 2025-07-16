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
            MainContent.Content = new MovieUpdateControl();
        }

        private void btnSnackInventory_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new SnackInventoryControl();
        }

        private void btnBookingMenu_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new BookingMenuControl();
        }
    }
}