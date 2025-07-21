using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using Microsoft.Data.SqlClient; // UPDATED: Using the new SQL client
using System.Configuration;

namespace MOVIETICKETING
{
    public partial class LoginWindow : Window
    {
        private string email = "";
        private string password = "";

        public LoginWindow()
        {
            InitializeComponent();
            UpdatePasswordPlaceholderVisibility();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            email = EmailTextBox.Text.Trim();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PasswordBox.Password;
            UpdatePasswordPlaceholderVisibility();
        }

        private void UpdatePasswordPlaceholderVisibility()
        {
            if (PasswordBox.Password.Length > 0)
            {
                PasswordPlaceholder.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordPlaceholder.Visibility = Visibility.Visible;
            }
        }

        private void LetsBingeButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthenticateUser(email, password, isAdmin: false))
            {
                MainWindow mainWindow = new MainWindow(email);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorText.Text = "Invalid email or password.";
            }
        }

        private void AdminLoginButton_Click(object sender, RoutedEventArgs e)
        {
            if (AuthenticateUser(email, password, isAdmin: true))
            {
                AdminDashboardWindow adminDashboard = new AdminDashboardWindow();
                adminDashboard.Show();
                this.Close();
            }
            else
            {
                ErrorText.Text = "Invalid admin credentials.";
            }
        }

        private bool AuthenticateUser(string email, string password, bool isAdmin)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            bool isAuthenticated = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM [dbo].[User] WHERE Email = @Email AND Password = @Password AND IsAdmin = @IsAdmin";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                        int count = (int)command.ExecuteScalar();
                        isAuthenticated = count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                isAuthenticated = false;
            }
            return isAuthenticated;
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? Visibility.Visible : Visibility.Collapsed;
            }
            if (value is string s)
            {
                return string.IsNullOrEmpty(s) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}