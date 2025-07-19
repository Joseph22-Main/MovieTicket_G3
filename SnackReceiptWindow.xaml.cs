using System.Windows;

namespace Pre_Order_Menu
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(List<string> orderedItems)
        {
            InitializeComponent();

            decimal total = 0;

            foreach (string item in orderedItems)
            {
                ReceiptListBox.Items.Add(item);

                var parts = item.Split('₱');
                if (parts.Length > 1 && decimal.TryParse(parts[1], out decimal price))
                {
                    total += price;
                }
            }

            TotalTextBlock.Text = $"Total: ₱{total:F2}";
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
