<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>README - Movie Ticketing System</title>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;700&display=swap" rel="stylesheet">
    <style>
        /* General Body Styles */
        body {
            font-family: 'Inter', -apple-system, BlinkMacSystemFont, "Segoe UI", Helvetica, Arial, sans-serif, "Apple Color Emoji", "Segoe UI Emoji";
            line-height: 1.6;
            color: #333;
            background-color: #f9f9f9;
            margin: 0;
            padding: 2rem;
        }

        /* Main Content Container */
        main {
            max-width: 800px;
            margin: 0 auto;
            background-color: #ffffff;
            border: 1px solid #e1e4e8;
            border-radius: 8px;
            padding: 2rem 3rem;
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        /* Headings */
        h1, h2, h3 {
            font-weight: 700;
            border-bottom: 2px solid #eaecef;
            padding-bottom: 0.5em;
            margin-top: 1.5em;
            margin-bottom: 1em;
        }

        h1 {
            font-size: 2.5em;
            text-align: center;
        }

        h2 {
            font-size: 2em;
        }

        h3 {
            font-size: 1.5em;
            border-bottom: 1px solid #eaecef;
        }

        /* Paragraphs and Lists */
        p, li {
            font-size: 1em;
            color: #444;
        }

        ul, ol {
            padding-left: 2em;
        }

        li {
            margin-bottom: 0.5em;
        }

        /* Links */
        a {
            color: #0366d6;
            text-decoration: none;
        }

        a:hover {
            text-decoration: underline;
        }

        /* Code Blocks */
        code {
            font-family: "SFMono-Regular", Consolas, "Liberation Mono", Menlo, Courier, monospace;
            background-color: #f6f8fa;
            padding: 0.2em 0.4em;
            border-radius: 4px;
            font-size: 0.9em;
        }

        /* Horizontal Rule */
        hr {
            border: 0;
            height: 1px;
            background-color: #d1d5da;
            margin: 2em 0;
        }

        /* Project Team Section */
        .project-team {
            text-align: center;
            font-style: italic;
            color: #586069;
            margin-top: 2em;
        }

        /* Responsive Design */
        @media (max-width: 768px) {
            body {
                padding: 1rem;
            }
            main {
                padding: 1.5rem;
            }
            h1 {
                font-size: 2em;
            }
        }
    </style>
</head>
<body>
    <main>
        <h1><strong>Movie Ticketing System</strong></h1>
        
        <p>A comprehensive, database-driven desktop application for booking movie tickets and managing a multi-theater cinema.</p>

        <hr>

        <h2><strong>Table of Contents</strong></h2>
        <ul>
            <li><a href="#overview">Overview</a></li>
            <li><a href="#key-features">Key Features</a></li>
            <li><a href="#technology-stack">Technology Stack</a></li>
            <li><a href="#getting-started">Getting Started</a></li>
            <li><a href="#project-team">Project Team</a></li>
        </ul>

        <h2 id="overview"><strong>Overview</strong></h2>
        <p>This project is a fully-featured Movie Ticketing System built with C# and WPF. It provides a robust, user-friendly interface for customers to browse movies, select showtimes, book seats in one of five theaters, and purchase snacks.</p>
        <p>For administrators, the system includes a powerful backend dashboard that allows for complete control over the cinema's operations, including movie and user management, showtime scheduling, and dynamic price adjustments for both tickets and snacks. The application is architected to be scalable and maintainable, with a clear separation between the user-facing client and the administrative backend.</p>

        <h2 id="key-features"><strong>Key Features</strong></h2>
        <h3>👤 <strong>User & Customer Features</strong></h3>
        <ul>
            <li><strong>Secure Authentication</strong>: Full user registration and login system with a "Change Password" feature.</li>
            <li><strong>Dynamic Movie & Showtime Browsing</strong>: View currently showing movies and select from available showtimes across multiple theaters.</li>
            <li><strong>Interactive Seat Selection</strong>: A visual 10x10 grid represents each of the 100 seats in a theater, with real-time status (available/reserved).</li>
            <li><strong>Integrated Snack Purchasing</strong>: Option to add snacks during ticket checkout or purchase them separately from a dedicated menu.</li>
            <li><strong>Reservation History</strong>: Users can view a detailed history of their past tickets and purchases in the "My Tickets" section.</li>
            <li><strong>Printable E-Receipts</strong>: Generates a detailed, printable e-receipt for every transaction.</li>
        </ul>

        <h3>🔒 <strong>Administrator Features</strong></h3>
        <ul>
            <li><strong>Centralized Management Dashboard</strong>: A secure, tab-based interface for managing all aspects of the system.</li>
            <li><strong>Full CRUD Functionality</strong>: Administrators have full Create, Read, Update, and Delete capabilities for:
                <ul>
                    <li><strong>Movies</strong>: Including titles, genres, posters, and ticket prices.</li>
                    <li><strong>Users</strong>: Add or remove customer and admin accounts.</li>
                    <li><strong>Snacks</strong>: Including names, images, and prices.</li>
                </ul>
            </li>
            <li><strong>Showtime Scheduling</strong>: Admins can schedule any movie to play in any of the five available theaters at a specific date and time.</li>
            <li><strong>Multi-Theater Architecture</strong>: The system is built to support five 100-seat theaters, with all reservations and showtimes managed at the database level.</li>
        </ul>

        <h2 id="technology-stack"><strong>Technology Stack</strong></h2>
        <ul>
            <li><strong>Application Framework</strong>: Windows Presentation Foundation (WPF)</li>
            <li><strong>Programming Language</strong>: C# (.NET)</li>
            <li><strong>Database</strong>: Microsoft SQL Server</li>
            <li><strong>Data Access</strong>: <code>Microsoft.Data.SqlClient</code> for modern, reliable database communication.</li>
            <li><strong>UI Design</strong>: XAML for declarative user interface design.</li>
        </ul>

        <h2 id="getting-started"><strong>Getting Started</strong></h2>
        <p>To set up and run this project locally, follow these steps:</p>
        <ol>
            <li><strong>Database Setup</strong>:
                <ul>
                    <li>Ensure you have an instance of Microsoft SQL Server running.</li>
                    <li>Execute the complete <code>sql_scripts.sql</code> file to create the <code>TicketingSystem</code> database, tables, and sample data.</li>
                </ul>
            </li>
            <li><strong>Connection String</strong>:
                <ul>
                    <li>Open the <code>App.config</code> file in the project.</li>
                    <li>Modify the <code>MovieTicketingDBConnection</code> connection string to point to your SQL Server instance and provide the correct credentials.</li>
                </ul>
            </li>
            <li><strong>Build & Run</strong>:
                <ul>
                    <li>Open the project solution in Visual Studio.</li>
                    <li>Build the solution to restore NuGet packages.</li>
                    <li>Run the application.</li>
                </ul>
            </li>
        </ol>

        <hr>

        <p class="project-team">This application was developed by <strong>Group 3</strong> for the CPE106-E01 course.</p>
    </main>
</body>
</html>
