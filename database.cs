using MySql.Data.MySqlClient;
using System;
using System.Data;

/// <summary>
/// A centralized class to handle all database connections and operations.
/// This class uses the connection string for your ticketingdb.
/// </summary>
public static class DatabaseHelper
{
    // IMPORTANT: Replace with your actual MySQL server details.
    // This connection string assumes your MySQL server is running on your local machine (localhost)
    // with the default port 3306, a user 'root', and a password 'password'.
    private const string ConnectionString = "Server=localhost;Port=3306;Database=ticketingdb;Uid=root;Pwd=password;";

    /// <summary>
    /// Establishes and opens a connection to the database.
    /// </summary>
    /// <returns>An open MySqlConnection object.</returns>
    public static MySqlConnection GetConnection()
    {
        try
        {
            MySqlConnection connection = new MySqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }
        catch (MySqlException ex)
        {
            // This will show a popup message if the database connection fails.
            // It's crucial for debugging connection issues.
            System.Windows.MessageBox.Show($"Database connection error: {ex.Message}\n\nPlease ensure your MySQL server is running and the connection string is correct.", "Database Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            return null;
        }
    }
}

/// <summary>
/// Represents the currently logged-in user.
/// Using a static class like this allows any part of the application
/// to know who is logged in.
/// </summary>
public static class AppState
{
    public static int CurrentUserId { get; private set; }
    public static string CurrentUsername { get; private set; }
    public static bool IsAdmin { get; private set; } // We'll assume all users in the DB are admins for now

    public static void Login(int userId, string username)
    {
        CurrentUserId = userId;
        CurrentUsername = username;
        IsAdmin = true; // For now, we assume anyone who logs in is an admin.
    }

    public static void Logout()
    {
        CurrentUserId = 0;
        CurrentUsername = null;
        IsAdmin = false;
    }
}
