using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MOVIETICKETING
{
    public partial class SeatSelectionWindow : Window
    {
        private string selectedMovie;
        private List<string> selectedSeats = new();

        public SeatSelectionWindow(string movie)
        {
            InitializeComponent();
            selectedMovie = movie;
            MovieTitle.Text = $"Select seats for: {movie}";
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string seat = btn.Content.ToString();

            if (!selectedSeats.Contains(seat))
            {
                selectedSeats.Add(seat);
                btn.Background = Brushes.LightGreen;
            }
            else
            {
                selectedSeats.Remove(seat);
                btn.Background = Brushes.LightGray;
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat.");
                return;
            }

            var receipt = new ReceiptWindow(selectedMovie, selectedSeats);
            receipt.ShowDialog();
            Close();
        }
    }
}
