using System.Collections.Generic;
using System.Windows;

namespace MOVIETICKETING
{
    public partial class SnackMenuWindow : Window
    {
        public SnackMenuWindow()
        {
            InitializeComponent();
        }

        private void ConfirmSnacks_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedSnacks = new();

            if (PopcornCheckBox.IsChecked == true) selectedSnacks.Add("Popcorn - ₱100");
            if (SodaCheckBox.IsChecked == true) selectedSnacks.Add("Soda - ₱50");
            if (NachosCheckBox.IsChecked == true) selectedSnacks.Add("Nachos - ₱120");
            if (HotdogCheckBox.IsChecked == true) selectedSnacks.Add("Hotdog - ₱80");

            var summary = new SnackOrderSummaryWindow(selectedSnacks);
            summary.ShowDialog();
            this.Close();
        }
    }
}
