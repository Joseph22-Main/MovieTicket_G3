using System.Windows;

namespace Pre_Order_Menu
{
    public partial class SnackOrderSummaryWindow : Window
    {
        private Dictionary<string, int> OrderItems;
        private Dictionary<string, decimal> ItemPrices;

        public SnackOrderSummaryWindow(Dictionary<string, int> items, Dictionary<string, decimal> prices)
        {
            InitializeComponent();
            OrderItems = new Dictionary<string, int>(items);
            ItemPrices = new Dictionary<string, decimal>(prices);

            DisplayOrder();
        }

        private void DisplayOrder()
        {
            OrderListBox.Items.Clear();
            decimal grandTotal = 0;

            foreach (var item in OrderItems)
            {
                string name = item.Key;
                int qty = item.Value;

                if (ItemPrices.TryGetValue(name, out decimal price))
                {
                    decimal subtotal = price * qty;
                    grandTotal += subtotal;

                    OrderListBox.Items.Add($"{name} x{qty} - ₱{subtotal:F2}");
                }
            }

            txtTotal.Text = $"Total: ₱{grandTotal:F2}";
            Title = $"Order Summary - Total: ₱{grandTotal:F2}";
        }

        private void ConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Finalize order?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Pass raw formatted items to receipt
                List<string> receiptItems = OrderItems.Select(item =>
                {
                    string name = item.Key;
                    int qty = item.Value;
                    decimal subtotal = ItemPrices[name] * qty;
                    return $"{name} x{qty} - ₱{subtotal:F2}";
                }).ToList();

                ReceiptWindow receipt = new ReceiptWindow(receiptItems);
                receipt.ShowDialog();
                MessageBox.Show("Order finalized!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }

        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListBox.SelectedItem != null)
            {
                string selected = OrderListBox.SelectedItem.ToString()!;
                string itemName = selected.Split('x')[0].Trim();

                string? input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Enter new quantity for {itemName}:", "Edit Item", OrderItems[itemName].ToString());

                if (int.TryParse(input, out int newQty) && newQty > 0)
                {
                    OrderItems[itemName] = newQty;
                    DisplayOrder();
                }
                else if (!string.IsNullOrWhiteSpace(input))
                {
                    MessageBox.Show("Invalid quantity. Please enter a positive number.", "Invalid Input");
                }
            }
            else
            {
                MessageBox.Show("Please select an item to edit.");
            }
        }

        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (OrderListBox.SelectedItem != null)
            {
                string selected = OrderListBox.SelectedItem.ToString()!;
                string itemName = selected.Split('x')[0].Trim();

                MessageBoxResult result = MessageBox.Show($"Delete '{itemName}'?", "Confirm Delete",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    OrderItems.Remove(itemName);
                    DisplayOrder();
                }
            }
            else
            {
                MessageBox.Show("Please select an item to delete.");
            }
        }
    }
}
