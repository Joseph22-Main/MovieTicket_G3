using System;
using System.Collections.Generic;
using System.Linq;

// =================================================================================
// DATA MODELS
// These classes define the structure for the data we will be working with.
// In a real application, this might be in separate files in a "Models" folder.
// =================================================================================

/// <summary>
/// Represents a movie available for ticketing.
/// </summary>
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public int DurationMinutes { get; set; }
    public decimal TicketPrice { get; set; }
}

/// <summary>
/// Represents a snack item available for purchase.
/// </summary>
public class Snack
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}

/// <summary>
/// Represents a user account (in this case, an admin).
/// </summary>
public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // Note: In a real app, passwords should be hashed!
}

/// <summary>
/// Represents a record of a successful login event.
/// </summary>
public class LoginRecord
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime LoginTime { get; set; }
}

/// <summary>
/// Represents a sold ticket, including movie, seats, and snacks.
/// </summary>
public class Ticket
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public List<string> Seats { get; set; } = new List<string>();
    public List<Snack> PurchasedSnacks { get; set; } = new List<Snack>();
    public decimal TotalCost { get; set; }
    public DateTime PurchaseTime { get; set; }
}


// =================================================================================
// MOCK DATABASE & SERVICES
// This section simulates the database and the services that interact with it.
// In a real application, this would be replaced with actual database calls (e.g., using Entity Framework or Dapper).
// =================================================================================

public static class TicketingSystem
{
    // --- Mock Database Tables ---
    private static List<AdminUser> Users { get; set; } = new List<AdminUser>();
    private static List<Movie> Movies { get; set; } = new List<Movie>();
    private static List<Snack> Snacks { get; set; } = new List<Snack>();
    private static List<LoginRecord> LoginRecords { get; set; } = new List<LoginRecord>();
    private static List<Ticket> SoldTickets { get; set; } = new List<Ticket>();
    private static int _nextTicketId = 1;

    /// <summary>
    /// Initializes the system with some sample data.
    /// </summary>
    public static void Initialize()
    {
        // Add a default admin user
        Users.Add(new AdminUser { Id = 1, Username = "admin", Password = "password123" });

        // Add some sample movies
        Movies.Add(new Movie { Id = 1, Title = "The Matrix", Genre = "Sci-Fi", DurationMinutes = 136, TicketPrice = 350.00m });
        Movies.Add(new Movie { Id = 2, Title = "Inception", Genre = "Sci-Fi", DurationMinutes = 148, TicketPrice = 375.00m });
        Movies.Add(new Movie { Id = 3, Title = "The Godfather", Genre = "Crime", DurationMinutes = 175, TicketPrice = 320.00m });

        // Add some sample snacks
        Snacks.Add(new Snack { Id = 1, Name = "Popcorn (Large)", Price = 150.00m });
        Snacks.Add(new Snack { Id = 2, Name = "Soda (Medium)", Price = 80.00m });
        Snacks.Add(new Snack { Id = 3, Name = "Nachos", Price = 120.00m });
    }

    // --- Feature Implementations ---

    /// <summary>
    /// 1. ADMIN LOGIN: Attempts to log in an admin.
    /// </summary>
    public static bool AdminLogin(string username, string password)
    {
        var user = Users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        if (user != null && user.Password == password)
        {
            // 4. SEEING LOG-IN RECORDS (Part 1: Creating the record)
            LoginRecords.Add(new LoginRecord
            {
                Id = LoginRecords.Count + 1,
                Username = username,
                LoginTime = DateTime.Now
            });
            Console.WriteLine("Login successful!");
            return true;
        }
        Console.WriteLine("Invalid username or password.");
        return false;
    }

    /// <summary>
    /// 2. ADMIN DASHBOARD (and other features)
    /// </summary>
    public static void ShowAdminDashboard()
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("       ADMIN DASHBOARD");
        Console.WriteLine("===================================");
        Console.WriteLine("1. View Movie List");
        Console.WriteLine("2. View Snack Menu");
        Console.WriteLine("3. View Login History");
        Console.WriteLine("4. Create New Ticket Sale");
        Console.WriteLine("5. Exit");
        Console.Write("\nSelect an option: ");
    }

    /// <summary>
    /// 5. MOVIES_LIST: Displays all movies.
    /// </summary>
    public static void DisplayMovies()
    {
        Console.WriteLine("\n--- Available Movies ---");
        foreach (var movie in Movies)
        {
            Console.WriteLine($"ID: {movie.Id} | Title: {movie.Title} | Price: {movie.TicketPrice:C}");
        }
    }

    /// <summary>
    /// 6. SNACKS: Displays all snacks.
    /// </summary>
    public static void DisplaySnacks()
    {
        Console.WriteLine("\n--- Available Snacks ---");
        foreach (var snack in Snacks)
        {
            Console.WriteLine($"ID: {snack.Id} | Name: {snack.Name} | Price: {snack.Price:C}");
        }
    }

    /// <summary>
    /// 4. SEEING LOG-IN RECORDS (Part 2: Viewing the records)
    /// </summary>
    public static void DisplayLoginRecords()
    {
        Console.WriteLine("\n--- Login History ---");
        if (!LoginRecords.Any())
        {
            Console.WriteLine("No login records found.");
            return;
        }
        foreach (var record in LoginRecords)
        {
            Console.WriteLine($"User: '{record.Username}' logged in at {record.LoginTime}");
        }
    }

    /// <summary>
    /// 3. TICKETING/SEATING ACCESS: Simulates creating a new ticket.
    /// </summary>
    public static void CreateTicketSale()
    {
        Console.WriteLine("\n--- New Ticket Sale ---");
        DisplayMovies();
        Console.Write("Enter the ID of the movie for the ticket: ");
        if (!int.TryParse(Console.ReadLine(), out int movieId))
        {
            Console.WriteLine("Invalid movie ID.");
            return;
        }

        var movie = Movies.FirstOrDefault(m => m.Id == movieId);
        if (movie == null)
        {
            Console.WriteLine("Movie not found.");
            return;
        }

        Console.Write("Enter seat numbers (e.g., A1,A2,B5): ");
        var seatInput = Console.ReadLine();
        var seats = seatInput.Split(',').Select(s => s.Trim().ToUpper()).ToList();

        decimal movieTotal = movie.TicketPrice * seats.Count;
        decimal snacksTotal = 0;
        var purchasedSnacks = new List<Snack>();

        Console.Write("Add snacks? (y/n): ");
        if (Console.ReadLine().ToLower() == "y")
        {
            while (true)
            {
                DisplaySnacks();
                Console.Write("Enter Snack ID to add (or 'done' to finish): ");
                string snackInput = Console.ReadLine();
                if (snackInput.ToLower() == "done") break;

                if (int.TryParse(snackInput, out int snackId))
                {
                    var snack = Snacks.FirstOrDefault(s => s.Id == snackId);
                    if (snack != null)
                    {
                        purchasedSnacks.Add(snack);
                        snacksTotal += snack.Price;
                        Console.WriteLine($"Added {snack.Name}. Current snack total: {snacksTotal:C}");
                    }
                    else
                    {
                        Console.WriteLine("Snack not found.");
                    }
                }
            }
        }

        var newTicket = new Ticket
        {
            Id = _nextTicketId++,
            MovieId = movie.Id,
            Seats = seats,
            PurchasedSnacks = purchasedSnacks,
            TotalCost = movieTotal + snacksTotal,
            PurchaseTime = DateTime.Now
        };

        SoldTickets.Add(newTicket);

        Console.WriteLine("\n--- TICKET CREATED SUCCESSFULLY ---");
        Console.WriteLine($"Ticket ID: {newTicket.Id}");
        Console.WriteLine($"Movie: {movie.Title}");
        Console.WriteLine($"Seats: {string.Join(", ", newTicket.Seats)}");
        Console.WriteLine($"Snacks: {string.Join(", ", newTicket.PurchasedSnacks.Select(s => s.Name))}");
        Console.WriteLine($"TOTAL COST: {newTicket.TotalCost:C}");
        Console.WriteLine("-----------------------------------");
    }
}

// =================================================================================
// PROGRAM ENTRY POINT
// This is where the console application starts.
// =================================================================================
public class Program
{
    public static void Main(string[] args)
    {
        // Set up the mock data
        TicketingSystem.Initialize();

        Console.WriteLine("Welcome to the Movie Ticketing System (Admin Console)");
        
        // 1. Admin Login
        bool isLoggedIn = false;
        while (!isLoggedIn)
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            isLoggedIn = TicketingSystem.AdminLogin(username, password);
        }

        // 2. Admin Dashboard Loop
        while (true)
        {
            TicketingSystem.ShowAdminDashboard();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": // 5. MOVIES_LIST
                    TicketingSystem.DisplayMovies();
                    break;
                case "2": // 6. SNACKS
                    TicketingSystem.DisplaySnacks();
                    break;
                case "3": // 4. SEEING LOG-IN RECORDS
                    TicketingSystem.DisplayLoginRecords();
                    break;
                case "4": // 3. TICKETING/SEATING ACCESS
                    TicketingSystem.CreateTicketSale();
                    break;
                case "5":
                    Console.WriteLine("Exiting application.");
                    return; // Exit the program
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to return to the dashboard...");
            Console.ReadKey();
        }
    }
}