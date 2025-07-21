Movie Ticketing System
A comprehensive, database-driven desktop application for booking movie tickets and managing a multi-theater cinema.

Table of Contents
Overview

Key Features

Technology Stack

Getting Started

Project Team

Overview
This project is a fully-featured Movie Ticketing System built with C# and WPF. It provides a robust, user-friendly interface for customers to browse movies, select showtimes, book seats in one of five theaters, and purchase snacks.

For administrators, the system includes a powerful backend dashboard that allows for complete control over the cinema's operations, including movie and user management, showtime scheduling, and dynamic price adjustments for both tickets and snacks. The application is architected to be scalable and maintainable, with a clear separation between the user-facing client and the administrative backend.

Key Features
👤 User & Customer Features
Secure Authentication: Full user registration and login system with a "Change Password" feature.

Dynamic Movie & Showtime Browsing: View currently showing movies and select from available showtimes across multiple theaters.

Interactive Seat Selection: A visual 10x10 grid represents each of the 100 seats in a theater, with real-time status (available/reserved).

Integrated Snack Purchasing: Option to add snacks during ticket checkout or purchase them separately from a dedicated menu.

Reservation History: Users can view a detailed history of their past tickets and purchases in the "My Tickets" section.

Printable E-Receipts: Generates a detailed, printable e-receipt for every transaction.

🔒 Administrator Features
Centralized Management Dashboard: A secure, tab-based interface for managing all aspects of the system.

Full CRUD Functionality: Administrators have full Create, Read, Update, and Delete capabilities for:

Movies: Including titles, genres, posters, and ticket prices.

Users: Add or remove customer and admin accounts.

Snacks: Including names, images, and prices.

Showtime Scheduling: Admins can schedule any movie to play in any of the five available theaters at a specific date and time.

Multi-Theater Architecture: The system is built to support five 100-seat theaters, with all reservations and showtimes managed at the database level.

Technology Stack
Application Framework: Windows Presentation Foundation (WPF)

Programming Language: C# (.NET)

Database: Microsoft SQL Server

Data Access: Microsoft.Data.SqlClient for modern, reliable database communication.

UI Design: XAML for declarative user interface design.

Getting Started
To set up and run this project locally, follow these steps:

Database Setup:

Ensure you have an instance of Microsoft SQL Server running.

Execute the complete sql_scripts.sql file to create the TicketingSystem database, tables, and sample data.

Connection String:

Open the App.config file in the project.

Modify the MovieTicketingDBConnection connection string to point to your SQL Server instance and provide the correct credentials.

Build & Run:

Open the project solution in Visual Studio.

Build the solution to restore NuGet packages.

Run the application.

This application was developed by Group 3 for the CPE106-E01 course.
