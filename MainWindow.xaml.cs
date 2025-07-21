using Movie_System;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Linq;


namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        // Connection string to your TicketingSystem database
        private readonly string connectionString = "Server=localhost;Database=TicketingSystem;Trusted_Connection=True;";
        private ObservableCollection<Movie> AllMovies = new ObservableCollection<Movie>();

        public MainWindow()
        {
            InitializeComponent();
            LoadMovies();
            ShowMovies("All");
        }

        private void LoadMovies()
        {
            AllMovies.Clear();
            AllMovies.Add(new Movie { Title = "Avengers", Genre = "Action", ImageUrl = "https://m.media-amazon.com/images/M/MV5BNGE0...jpg" });
            AllMovies.Add(new Movie { Title = "Despicable Me", Genre = "Cartoon", ImageUrl = "https://play-lh.googleusercontent.com/10M4F-ok0cYT...jpg" });
            AllMovies.Add(new Movie { Title = "Forrest Gump", Genre = "Romance", ImageUrl = "https://m.media-amazon.com/images/S/...jpg" });
            AllMovies.Add(new Movie { Title = "Sausage Party", Genre = "Horror", ImageUrl = "https://upload.wikimedia.org/wikipedia/en/...png" });
        }

        private void ShowMovies(string genre)
        {
            var filtered = genre == "All" ? AllMovies : new ObservableCollection<Movie>(AllMovies.Where(m => m.Genre == genre));

            var panel = new UniformGrid { Columns = 2, Rows = 2 };

            foreach (var movie in filtered)
            {
                var stack = new StackPanel();

                var image = new Image
                {
                    Source = new BitmapImage(new Uri(movie.ImageUrl)),
                    Height = 200,
                    Stretch = Stretch.UniformToFill
                };

                var text = new TextBlock
                {
                    Text = movie.Title + " (" + movie.Genre + ")",
                    Foreground = Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                stack.Children.Add(image);
                stack.Children.Add(text);
                panel.Children.Add(stack);
            }

            MainContent.Content = panel;
        }

        private void Genre_Click(object sender, RoutedEventArgs e)
        {
            string genre = (sender as Button).Content.ToString();
            ShowMovies(genre);
        }

        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            MainContent.ContentTemplate = (DataTemplate)FindResource("RecommendedView");
            HighlightSidebar(RecommendedButton);
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            // Clear existing view and load movies from database
            MainContent.ContentTemplate = null;
            LoadMoviesFromDatabase();
            HighlightSidebar(MoviesButton);
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainContent.ContentTemplate = (DataTemplate)FindResource("AccountView");
            HighlightSidebar(AccountButton);
        }

        private void HighlightSidebar(Button selectedButton)
        {
            // Reset background for all
            RecommendedButton.Background = Brushes.Transparent;
            MoviesButton.Background = Brushes.Transparent;
            AccountButton.Background = Brushes.Transparent;

            // Highlight the selected one
            selectedButton.Background = new SolidColorBrush(Color.FromRgb(35, 54, 73));
        }

        private void LoadMoviesFromDatabase()
        {
            var panel = new UniformGrid { Columns = 2, Margin = new Thickness(20) };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Title, ImageUrl FROM Movies";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        string title = reader["Title"].ToString();
                        string imageUrl = reader["ImageUrl"].ToString();

                        var stack = new StackPanel();

                        var image = new Image
                        {
                            Source = new BitmapImage(new Uri(imageUrl, UriKind.Absolute)),
                            Stretch = Stretch.UniformToFill,
                            ClipToBounds = true
                        };

                        var border = new Border
                        {
                            Width = 500,
                            Height = 500,
                            Background = Brushes.Transparent,
                            CornerRadius = new CornerRadius(15),
                            Child = image
                        };

                        var text = new TextBlock
                        {
                            Text = title,
                            Foreground = Brushes.White,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            Margin = new Thickness(5)
                        };

                        stack.Children.Add(border);
                        stack.Children.Add(text);

                        var button = new Button
                        {
                            Background = Brushes.Transparent,
                            BorderThickness = new Thickness(0),
                            Content = stack
                        };

                        button.Click += OpenSeatSelection;

                        panel.Children.Add(button);
                    }

                    reader.Close();
                }

                // Set the dynamically created content
                MainContent.Content = panel;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading movies: " + ex.Message);
            }
        }
        private void OpenSeatSelection(object sender, RoutedEventArgs e)
        {
            // Grab the movie title from the clicked button's content
            if (sender is Button btn && btn.Content is StackPanel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is TextBlock textBlock)
                    {
                        string movieTitle = textBlock.Text;

                        // Open Seat Selection Window with the selected movie
                        SeatSelectionWindow seatWindow = new SeatSelectionWindow(movieTitle);
                        seatWindow.ShowDialog();
                        break;
                    }
                }
            }
        }


    }
}
