using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Data.SqlClient; // UPDATED: Using the new SQL client
using System.Configuration;

namespace MOVIETICKETING
{
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();
        private string userEmail = ""; // FIX: Initialized to prevent null warning
        private List<Movie> currentMovies = new List<Movie>();

        public MainWindow(string email)
        {
            InitializeComponent();
            this.userEmail = email;
            LoadMoviesFromDatabase();
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadMoviesFromDatabase();
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
                    string query = "SELECT MovieID, Title, Genre, Duration, DateRelease, ImageFile FROM [dbo].[Movie]";
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
                                    ImageFile = reader.GetString(reader.GetOrdinal("ImageFile"))
                                });
                            }
                        }
                    }
                }
                DisplayMovies(currentMovies);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading movies from database: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                MainContent.Children.Clear();
                MainContent.Children.Add(new TextBlock
                {
                    Text = "Could not load movies. Please check database connection.",
                    Foreground = Brushes.Red,
                    FontSize = 18,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
        }

        // ... (No changes to other methods like Recommended_Click, Movies_Click, etc.)
        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            var recommended = currentMovies.OrderBy(_ => random.Next()).Take(2).ToList();
            DisplayMovies(recommended);
            HighlightSidebar(RecommendedButton);
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            DisplayMovies(currentMovies);
            HighlightSidebar(MoviesButton);
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center };

            stack.Children.Add(new TextBlock
            {
                Text = "User Account",
                FontSize = 22,
                Foreground = Brushes.Black,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            stack.Children.Add(new TextBlock
            {
                Text = $"Email: {userEmail}",
                FontSize = 16,
                Foreground = Brushes.Gray,
                Margin = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center
            });

            MainContent.Children.Add(stack);
            HighlightSidebar(AccountButton);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Children.Clear();
            string query = SearchBox.Text.Trim();
            var found = currentMovies.FirstOrDefault(m => m.Title.Equals(query, StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                DisplayMovies(new List<Movie> { found });
            }
            else
            {
                MainContent.Children.Add(new TextBlock
                {
                    Text = $"No '{query}' found.",
                    Foreground = Brushes.Black,
                    FontSize = 24,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                });
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        private void DisplayMovies(List<Movie> movieList)
        {
            var panel = new WrapPanel { Margin = new Thickness(10) };

            foreach (var movie in movieList)
            {
                var stack = new StackPanel { Margin = new Thickness(10), Width = 220 };

                ImageSource movieImageSource = null;
                try
                {
                    string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", movie.ImageFile);
                    movieImageSource = new BitmapImage(new Uri(imagePath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading image {movie.ImageFile}: {ex.Message}");
                    movieImageSource = new BitmapImage(new Uri("https://placehold.co/220x260/CCCCCC/000000?text=No+Image"));
                }

                var image = new Image
                {
                    Source = movieImageSource,
                    Height = 260,
                    Stretch = Stretch.UniformToFill
                };

                var text = new TextBlock
                {
                    Text = $"{movie.Title} ({movie.Genre}) - {movie.Duration}m",
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5),
                    TextWrapping = TextWrapping.Wrap
                };

                stack.Children.Add(image);
                stack.Children.Add(text);
                panel.Children.Add(stack);
            }

            MainContent.Children.Clear();
            MainContent.Children.Add(panel);
        }

        private string RandomizeTime()
        {
            var hour = random.Next(10, 23);
            var minute = random.Next(0, 60);
            return $"{hour:D2}:{minute:D2}";
        }

        private void HighlightSidebar(Button selected)
        {
            RecommendedButton.Background = Brushes.Transparent;
            MoviesButton.Background = Brushes.Transparent;
            AccountButton.Background = Brushes.Transparent;

            selected.Background = new SolidColorBrush(Color.FromRgb(167, 201, 87));
        }
    }

    public class Movie
    {
        public int MovieID { get; set; }
        // FIX: Initialized properties to satisfy non-nullable warnings
        public string Title { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public int Duration { get; set; }
        public DateTime DateRelease { get; set; }
        public string ImageFile { get; set; } = string.Empty;
    }
}