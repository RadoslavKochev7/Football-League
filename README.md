# Football League

A .NET 8 Web API application for managing football teams, matches, and league standings.

## Features

- Manage teams (add, edit, delete)
- Record and update matches
- Calculate league standings
- RESTful API endpoints
- Unit tests

## Technologies

- .NET 8 (C# 12)
- ASP.NET Core Web API
- Entity Framework Core
- NUnit & Moq for testing

## Getting Started

1. **Clone the repository:**
git clone https://github.com/RadoslavKochev7/Football-League.git cd Football-League

2. **Update the database:**
CLI: dotnet ef database update --project FootballLeague.Infrastructure --startup-project FootballLeague.API
Package Manager Console: Update-Database -StartupProject FootballLeague.API

3. **Run the API:**
dotnet run --project FootballLeague.API

4. **Run tests:**
dotnet test

## Project Structure

- `FootballLeague.API` - API endpoints and startup
- `FootballLeague.Core` - Business logic and domain models
- `FootballLeague.Infrastructure` - Data access and migrations
- `FootballLeague.Tests` - Unit and integration tests
