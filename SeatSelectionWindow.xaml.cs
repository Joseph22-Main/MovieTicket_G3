using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Movie_System
{
    public partial class SeatSelectionWindow : Window
    {
        private List<string> selectedSeats = new List<string>();
        private string connectionString = "Data Source=localhost;Initial Catalog=TicketingSystem;Integrated Security=True";
        private string currentMovie = "Avengers"; // You can pass this in from MainWindow

        public SeatSelectionWindow(string movieTitle)
        {
            InitializeComponent();
            currentMovie = movieTitle;
            InitializeSeats();
        }

        private void InitializeSeats()
        {
            string[] rows = { "A", "B", "C", "D", "E" };
            List<string> reservedSeats = GetReservedSeatsFromDatabase();

            foreach (string row in rows)
            {
                for (int i = 1; i <= 5; i++)
                {
                    string seatId = row + i;
                    Button seatButton = new Button
                    {
                        Content = seatId,
                        Style = (Style)FindResource("SeatButtonStyle"),
                        Tag = seatId
                    };

                    // If reserved, disable and mark red
                    if (reservedSeats.Contains(seatId))
                    {
                        seatButton.Background = Brushes.Red;
                        seatButton.IsEnabled = false;
                    }
                    else
                    {
                        seatButton.Click += SeatButton_Click;
                    }

                    SeatGrid.Children.Add(seatButton);
                }
            }
        }

        private List<string> GetReservedSeatsFromDatabase()
        {
            List<string> reserved = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SeatID FROM Reservations WHERE MovieTitle = @title AND Reserved = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", currentMovie);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reserved.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return reserved;
        }

        private void SeatButton_Click(object sender, RoutedEventArgs e)
        {
            Button seat = sender as Button;
            string seatId = seat.Tag.ToString();

            if (!selectedSeats.Contains(seatId))
            {
                selectedSeats.Add(seatId);
                seat.Background = Brushes.Green;
            }
            else
            {
                selectedSeats.Remove(seatId);
                seat.ClearValue(Button.BackgroundProperty);
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("Please select at least one seat.");
                return;
            }

            // Optionally save to database here

            ReceiptWindow receipt = new ReceiptWindow(selectedSeats);
            receipt.ShowDialog();
            this.Close();
        }
    }
