using System.Windows;

namespace MOVIETICKETING
{
    public partial class LoginWindow : Window
    {
        private string email = "";
        private string password = "";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void EmailTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            email = EmailTextBox.Text.Trim();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            password = PasswordBox.Password;
        }

        private void LetsBingeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValidLogin(email, password))
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                ErrorText.Text = "Only @gmail.com emails are accepted.";
            }
        }

        private bool IsValidLogin(string email, string password)
        {
            return email.EndsWith("@gmail.com") && !string.IsNullOrWhiteSpace(password);
        }
    }
}
