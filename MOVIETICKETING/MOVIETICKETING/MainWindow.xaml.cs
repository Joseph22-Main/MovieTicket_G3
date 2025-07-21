using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MOVIETICKETING
{
    public partial class MainWindow : Window
    {
        private string userEmail = "";
        private int userID = -1;
        private List<Movie> currentMovies = new List<Movie>();

        public MainWindow(string email)
        {
            InitializeComponent();
            this.userEmail = email;
            GetUserDetails();
            LoadMoviesFromDatabase();
        }

        private void GetUserDetails()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            string query = "SELECT UserID FROM [dbo].[User] WHERE Email = @Email";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", this.userEmail);
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            this.userID = (int)result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not fetch user details: {ex.Message}", "Error");
            }
        }

        private void LoadMoviesFromDatabase()
        {
            currentMovies.Clear();
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT MovieID, Title, Genre, Duration, DateRelease, ImageFile, Price FROM [dbo].[Movie]";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                currentMovies.Add(new Movie
                                {
                                    MovieID = reader.GetInt32(reader.GetOrdinal("MovieID")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Genre = reader.GetString(reader.GetOrdinal("Genre")),
                                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                    DateRelease = reader.GetDateTime(reader.GetOrdinal("DateRelease")),
                                    ImageFile = reader.GetString(reader.GetOrdinal("ImageFile")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                                });
                            }
                        }
                    }
                }
                DisplayMovies(currentMovies);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movies: {ex.Message}", "Database Error");
            }
        }

        private void DisplayMovies(List<Movie> movies)
        {
            MainContent.ItemsSource = movies;
        }

        private void Movie_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Movie selectedMovie)
            {
                ShowtimeSelectionWindow showtimeWindow = new ShowtimeSelectionWindow(selectedMovie, this.userID);
                showtimeWindow.ShowDialog();
            }
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            DisplayMovies(currentMovies);
            HighlightSidebar(MoviesButton);
        }

        private void MyTickets_Click(object sender, RoutedEventArgs e)
        {
            MyTicketsWindow ticketsWindow = new MyTicketsWindow(this.userID);
            ticketsWindow.ShowDialog();
            HighlightSidebar(MyTicketsButton);
        }

        private void Snacks_Click(object sender, RoutedEventArgs e)
        {
            HighlightSidebar(SnacksButton);

            if (this.userID == -1)
            {
                MessageBox.Show("Could not verify user. Please log in again.", "Authentication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var snackMenu = new SnackMenuWindow(this.userID);
            if (snackMenu.ShowDialog() == true && snackMenu.SelectedSnacks.Any())
            {
                var snacks = snackMenu.SelectedSnacks;
                decimal totalPrice = snacks.Sum(s => s.Price * s.Quantity);

                string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
                try
                {
                    using (var conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        foreach (var snack in snacks)
                        {
                            string query = "INSERT INTO PurchasedSnack (ReservationID, SnackID, Quantity, PurchaseDate, UserID) VALUES (NULL, @SnackID, @Quantity, @Date, @UserID)";
                            using (var cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@SnackID", snack.SnackID);
                                cmd.Parameters.AddWithValue("@Quantity", snack.Quantity);
                                cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                                cmd.Parameters.AddWithValue("@UserID", this.userID);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    var receiptDetails = new Reservation
                    {
                        MovieTitle = "Snacks Only Purchase",
                        ReservationDate = DateTime.Now,
                        Snacks = snacks,
                        TotalPrice = totalPrice
                    };
                    var receipt = new ReceiptWindow(receiptDetails);
                    receipt.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save snack purchase: {ex.Message}");
                }
            }
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow(this.userEmail);
            accountWindow.ShowDialog();
            HighlightSidebar(AccountButton);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string query = SearchBox.Text.Trim();
            var foundMovies = currentMovies.Where(m => m.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();
            DisplayMovies(foundMovies);
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void HighlightSidebar(Button selected)
        {
            MoviesButton.Background = Brushes.Transparent;
            MyTicketsButton.Background = Brushes.Transparent;
            SnacksButton.Background = Brushes.Transparent;
            AccountButton.Background = Brushes.Transparent;
            selected.Background = new SolidColorBrush(Color.FromRgb(167, 201, 87));
        }
    }
}
