using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace MOVIETICKETING
{
    public partial class SnackMenuWindow : Window
    {
        public List<Snack> SelectedSnacks { get; private set; }
        private List<Snack> _availableSnacks = new List<Snack>();
        private readonly int _userID;

        public SnackMenuWindow(int userID)
        {
            InitializeComponent();
            SelectedSnacks = new List<Snack>();
            _userID = userID;
            LoadSnacks();
        }

        private void LoadSnacks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieTicketingDBConnection"].ConnectionString;
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT SnacksID, NameSnack, Price FROM [dbo].[SnackShop]";
                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _availableSnacks.Add(new Snack
                                {
                                    SnackID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Price = Convert.ToDecimal(reader.GetValue(2)),
                                    Quantity = 1
                                });
                            }
                        }
                    }
                }
                SnacksItemsControl.ItemsSource = _availableSnacks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load snacks: {ex.Message}", "Database Error");
            }
        }

        private void ConfirmSnacks_Click(object sender, RoutedEventArgs e)
        {
            SelectedSnacks = _availableSnacks.Where(s => s.IsSelected && s.Quantity > 0).ToList();

            this.DialogResult = true;
            this.Close();
        }
    }
}
