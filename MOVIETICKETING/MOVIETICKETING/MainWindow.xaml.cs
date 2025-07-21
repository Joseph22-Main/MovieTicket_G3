using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MOVIETICKETING
{
    public partial class MainWindow : Window
    {
        private readonly List<Movie> movies = new List<Movie>
        {
            new Movie { Title = "Avengers", Duration = "2h 23m", ImageFile = "avengers.jpg" },
            new Movie { Title = "Despicable Me", Duration = "1h 35m", ImageFile = "despicable.jpg" },
            new Movie { Title = "Forrest Gump", Duration = "2h 22m", ImageFile = "forrest.jpg" },
            new Movie { Title = "Sausage Party", Duration = "1h 29m", ImageFile = "sausage.jpg" },
            new Movie { Title = "Fifty Shades of Grey", Duration = "2h 5m", ImageFile = "fifty.jpg" }
        };

        private readonly Random random = new Random();
        private string userEmail;

        public MainWindow(string email)
        {
            InitializeComponent();
            this.userEmail = email;
            ShowAllMovies();
        }

        public MainWindow()
        {
            InitializeComponent();
            ShowAllMovies();
        }

        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            var recommended = movies.OrderBy(_ => random.Next()).Take(2).ToList();
            DisplayMovies(recommended);
            HighlightSidebar(RecommendedButton);
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            ShowAllMovies();
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
            var found = movies.FirstOrDefault(m => m.Title.Equals(query, StringComparison.OrdinalIgnoreCase));

            if (found != null)
            {
                DisplayMovies(new List<Movie> { found });
            }
            else
            {
                MainContent.Children.Add(new TextBlock
                {
                    Text = $"No {query} found.",
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

        private void ShowAllMovies()
        {
            DisplayMovies(movies);
        }

        private void DisplayMovies(List<Movie> movieList)
        {
            var panel = new WrapPanel { Margin = new Thickness(10) };

            foreach (var movie in movieList)
            {
                var stack = new StackPanel { Margin = new Thickness(10), Width = 220 };

                var image = new Image
                {
                    Source = new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images", movie.ImageFile))),
                    Height = 260,
                    Stretch = Stretch.UniformToFill
                };

                var text = new TextBlock
                {
                    Text = $"{movie.Title} - {RandomizeTime()} • {movie.Duration}",
                    Foreground = Brushes.Black,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(5)
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
        public string Title { get; set; }
        public string Duration { get; set; }
        public string ImageFile { get; set; }
    }
}