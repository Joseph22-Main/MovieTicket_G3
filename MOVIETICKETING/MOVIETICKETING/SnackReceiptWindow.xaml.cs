using System.Collections.Generic;
using System.Windows;

namespace MOVIETICKETING
{
    public partial class SnackReceiptWindow : Window
    {
        public SnackReceiptWindow(List<string> snacks)
        {
            InitializeComponent();

            double total = 0;
            foreach (var item in snacks)
            {
                ReceiptText.Text += $"{item}\n";

                if (item.Contains("Popcorn")) total += 100;
                else if (item.Contains("Soda")) total += 50;
                else if (item.Contains("Nachos")) total += 120;
                else if (item.Contains("Hotdog")) total += 80;
            }

            TotalText.Text = $"Total: ₱{total}";
        }
    }
}
