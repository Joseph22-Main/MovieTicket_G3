using System.Collections.Generic;
using System.Windows;

namespace MOVIETICKETING
{
    public partial class ReceiptWindow : Window
    {
        public ReceiptWindow(string movie, List<string> seats)
        {
            InitializeComponent();
            TitleText.Text = $"Movie: {movie}";
            SeatsText.Text = $"Seats: {string.Join(", ", seats)}";
            TotalText.Text = $"Total: ₱{seats.Count * 250}";
        }
    }
}
