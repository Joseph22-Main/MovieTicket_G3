using System.Collections.Generic;
using System.Windows;

namespace Movie_System
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(List<string> selectedSeats)
        {
            InitializeComponent();

            int seatCount = selectedSeats.Count;
            int pricePerSeat = 200;
            int total = seatCount * pricePerSeat;

            SelectedSeatsText.Text = "Seats: " + string.Join(", ", selectedSeats);
            SeatCountText.Text = "Total Seats: " + seatCount;
            TotalAmountText.Text = "Total Amount: PHP " + total;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
