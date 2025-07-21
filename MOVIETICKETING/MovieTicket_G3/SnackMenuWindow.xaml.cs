using System.Windows;
using System.Windows.Controls;

namespace Pre_Order_Menu
{
    public partial class SnackMenuWindow : Window
    {
        private readonly Dictionary<string, decimal> itemPrices = new()
        {
            { "Popcorn", 80.00m },
            { "Soda", 50.00m },
            { "Nachos", 100.00m },
            { "Hotdogs", 70.00m },
            { "Fries", 60.00m }
        };

        private Dictionary<string, int> confirmedQuantities = new();

        public SnackMenuWindow()
        {
            InitializeComponent();
        }

        private void IncreaseQuantity(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                UpdateQuantity(tag, +1);
        }

        private void DecreaseQuantity(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
                UpdateQuantity(tag, -1);
        }

        private void UpdateQuantity(string item, int delta)
        {
            TextBox? targetBox = item switch
            {
                "popcorn" => txtPopcorn,
                "soda" => txtSoda,
                "nachos" => txtNachos,
                "hotdogs" => txtHotdogs,
                "fries" => txtFries,
                _ => null
            };

            if (targetBox != null && int.TryParse(targetBox.Text, out int current))
                targetBox.Text = Math.Max(0, current + delta).ToString();
        }

        private void AddToOrder_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<string, int> currentSelection = new();

            if (int.TryParse(txtPopcorn.Text, out int pop) && pop > 0)
                currentSelection["Popcorn"] = pop;

            if (int.TryParse(txtSoda.Text, out int soda) && soda > 0)
                currentSelection["Soda"] = soda;

            if (int.TryParse(txtNachos.Text, out int nachos) && nachos > 0)
                currentSelection["Nachos"] = nachos;

            if (int.TryParse(txtHotdogs.Text, out int hotdogs) && hotdogs > 0)
                currentSelection["Hotdogs"] = hotdogs;

            if (int.TryParse(txtFries.Text, out int fries) && fries > 0)
                currentSelection["Fries"] = fries;

            if (currentSelection.Count == 0)
            {
                MessageBox.Show("No items selected.");
                return;
            }

            // Merge into confirmedQuantities
            foreach (var item in currentSelection)
            {
                if (confirmedQuantities.ContainsKey(item.Key))
                    confirmedQuantities[item.Key] += item.Value;
                else
                    confirmedQuantities[item.Key] = item.Value;
            }

            // Optional: Feedback for added items
            string addedSummary = string.Join("\n", currentSelection.Select(item =>
                $"{item.Key} x{item.Value} - ₱{item.Value * itemPrices[item.Key]:0}"));
            MessageBox.Show("You added:\n" + addedSummary);

            // Reset quantities
            txtPopcorn.Text = "0";
            txtSoda.Text = "0";
            txtNachos.Text = "0";
            txtHotdogs.Text = "0";
            txtFries.Text = "0";
        }


        private void ShowList_Click(object sender, RoutedEventArgs e)
        {
            if (confirmedQuantities.Count == 0)
            {
                MessageBox.Show("No items in the order.");
                return;
            }

            SnackOrderSummaryWindow summaryWindow = new SnackOrderSummaryWindow(confirmedQuantities, itemPrices);
            summaryWindow.Show();
        }
    }
}
