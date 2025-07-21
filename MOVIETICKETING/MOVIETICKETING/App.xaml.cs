using System.Windows;

namespace MOVIETICKETING
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Show login first
            var login = new LoginWindow();
            login.Show();
        }
    }
}
