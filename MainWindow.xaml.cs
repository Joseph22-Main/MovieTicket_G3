using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace main
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Recommended_Click(object sender, RoutedEventArgs e)
        {
            MainContent.ContentTemplate = (DataTemplate)FindResource("RecommendedView");
            HighlightSidebar(RecommendedButton);
        }

        private void Movies_Click(object sender, RoutedEventArgs e)
        {
            MainContent.ContentTemplate = (DataTemplate)FindResource("MoviesView");
            HighlightSidebar(MoviesButton);
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainContent.ContentTemplate = (DataTemplate)FindResource("AccountView");
            HighlightSidebar(AccountButton);
        }

        private void HighlightSidebar(Button selectedButton)
        {
            RecommendedButton.Background = Brushes.Transparent;
            MoviesButton.Background = Brushes.Transparent;
            AccountButton.Background = Brushes.Transparent;

            selectedButton.Background = new SolidColorBrush(Color.FromRgb(35, 54, 73)); // #233649
        }
    }
}
