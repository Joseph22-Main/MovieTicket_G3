using System.Collections.Generic;
using System.Windows;

namespace MOVIETICKETING
{
    public partial class SnackOrderSummaryWindow : Window
    {
        private List<string> snacks;

        public SnackOrderSummaryWindow(List<string> selectedSnacks)
        {
            InitializeComponent();
            snacks = selectedSnacks;

            foreach (var snack in snacks)
            {
                SnackListBox.Items.Add(snack);
            }
        }

        private void GenerateReceipt_Click(object sender, RoutedEventArgs e)
        {
            var receipt = new SnackReceiptWindow(snacks);
            receipt.ShowDialog();
            this.Close();
        }
    }
}
