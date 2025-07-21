using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Linq;

namespace MOVIETICKETING
{
    public partial class SeatSelectionWindow : Window
    {
        private class SeatViewModel : Seat
        {
            public Brush BackgroundColor { get; set; } = Brushes.LightGray;
            public bool IsEnabled => !IsReserved;
        }

        private readonly Showtime _showtime;
        private readonly int _userID;
        private readonly List<SeatViewModel> _selectedSeats = new List<SeatViewModel>();
        private readonly List<SeatViewModel> _allSeats = new List<SeatViewModel>();

        public SeatSelectionWindow(Showtime showtime, int userID)
        {
            InitializeComponent();
            _showtime = showtime;
            _userID = userID;
            MovieTitle.Text = $"Select seats for: {showtime.MovieTitle}";
            LoadSeats();
        }

        private void LoadSeats()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT s.SeatID, s.SeatNumber, CASE WHEN rs.ReservedSeatID IS NOT NULL THEN 1 ELSE 0 END AS IsReserved
                                     FROM Seat s
                                     LEFT JOIN ReservedSeat rs ON s.SeatID = rs.SeatID
                                     LEFT JOIN Reservation r ON rs.ReservationID = r.ReservationID AND r.ShowtimeID = @ShowtimeID
                                     WHERE s.TheaterID = (SELECT TheaterID FROM Showtime WHERE ShowtimeID = @ShowtimeID)";
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ShowtimeID", _showtime.ShowtimeID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var seat = new SeatViewModel
                                {
                                    SeatID = reader.GetInt32(0),
                                    SeatNumber = reader.GetString(1),
                                    IsReserved = reader.GetInt32(2) == 1
                                };
                                if (seat.IsReserved)
                                {
                                    seat.BackgroundColor = Brushes.Red;
                                }
                                _allSeats.Add(seat);
                            }
                        }
                    }
                }
                SeatGrid.ItemsSource = _allSeats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load seats: {ex.Message}");
            }
        }

        private void Seat_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is SeatViewModel seat)
            {
                if (_selectedSeats.Contains(seat))
                {
                    _selectedSeats.Remove(seat);
                    btn.Background = Brushes.LightGray;
                }
                else
                {
                    _selectedSeats.Add(seat);
                    btn.Background = Brushes.LightGreen;
                }
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat.");
                return;
            }

            var snacks = new List<Snack>();
            if (MessageBox.Show("Would you like to add snacks?", "Snacks", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var snackMenu = new SnackMenuWindow(_userID);
                if (snackMenu.ShowDialog() == true)
                {
                    snacks = snackMenu.SelectedSnacks;
                }
            }

            SaveReservation(snacks);
        }

        private void SaveReservation(List<Snack> snacks)
        {
            var moviePrice = GetMoviePrice(_showtime.MovieID);
            decimal totalSnackPrice = snacks.Sum(s => s.Price * s.Quantity);
            decimal totalTicketPrice = moviePrice * _selectedSeats.Count;
            decimal finalPrice = totalTicketPrice + totalSnackPrice;

            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            int reservationID = -1;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string resQuery = "INSERT INTO Reservation (UserID, ShowtimeID, ReservationDate, TotalPrice) OUTPUT INSERTED.ReservationID VALUES (@UserID, @ShowtimeID, @Date, @TotalPrice);";
                    using (var cmd = new SqlCommand(resQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", _userID);
                        cmd.Parameters.AddWithValue("@ShowtimeID", _showtime.ShowtimeID);
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@TotalPrice", finalPrice);
                        reservationID = (int)cmd.ExecuteScalar();
                    }

                    foreach (var seat in _selectedSeats)
                    {
                        string seatQuery = "INSERT INTO ReservedSeat (ReservationID, SeatID) VALUES (@ReservationID, @SeatID);";
                        using (var cmd = new SqlCommand(seatQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@ReservationID", reservationID);
                            cmd.Parameters.AddWithValue("@SeatID", seat.SeatID);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                var reservationDetails = new Reservation
                {
                    MovieTitle = _showtime.MovieTitle,
                    ReservationDate = DateTime.Now,
                    Seats = _selectedSeats.Select(s => s.SeatNumber).ToList(),
                    Snacks = snacks,
                    TotalPrice = finalPrice
                };
                var receipt = new ReceiptWindow(reservationDetails);
                receipt.ShowDialog();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save reservation: {ex.Message}", "Database Error");
            }
        }

        private decimal GetMoviePrice(int movieID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            string query = "SELECT Price FROM Movie WHERE MovieID = @MovieID";
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@MovieID", movieID);
                        return (decimal)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception)
            {
                return 250.00m;
            }
        }
    }
}
