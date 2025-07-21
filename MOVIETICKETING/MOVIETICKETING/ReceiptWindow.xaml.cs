using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;

namespace MOVIETICKETING
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(Reservation reservation)
        {
            InitializeComponent();
            DataContext = reservation;

            var culture = new CultureInfo("en-PH");

            TransactionDateText.Text = $"Date: {reservation.ReservationDate:yyyy-MM-dd HH:mm:ss}";
            MovieTitleText.Text = $"Movie: {reservation.MovieTitle}";
            SeatsText.Text = $"Seats: {string.Join(", ", reservation.Seats)}";

            decimal ticketPrice = reservation.TotalPrice;
            if (reservation.Snacks.Count > 0)
            {
                foreach (var snack in reservation.Snacks)
                {
                    ticketPrice -= snack.Price * snack.Quantity;
                }
            }
            TicketPriceText.Text = $"Ticket Price: {ticketPrice.ToString("C", culture)}";

            if (reservation.Snacks.Count > 0)
            {
                SnacksItemsControl.ItemsSource = reservation.Snacks;
            }
            else
            {
                NoSnacksText.Visibility = Visibility.Visible;
                SnacksItemsControl.Visibility = Visibility.Collapsed;
            }

            TotalAmountText.Text = $"TOTAL AMOUNT: {reservation.TotalPrice.ToString("C", culture)}";
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(PrintArea, "Movie Ticket E-Receipt");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while printing: {ex.Message}", "Print Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
