# Football League

A .NET 8 Web API application for managing football teams, matches, and league standings.

## Features

- Manage teams and matches
- Record and update matches
- Calculate league standings
- RESTful API endpoints
- Unit tests

## Technologies

- .NET 8 (C# 12)
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- NUnit & Moq for testing
- FluentValidation
- Swagger

## Entities
- Team
- Match

## Getting Started

1. **Clone the repository:**
-git clone https://github.com/RadoslavKochev7/Football-League.git
-cd Football-League

2. **Update the database:**

- Assure dotnet ef is installed or run dotnet tool install --global dotnet-ef
- CLI: dotnet ef database update --project FootballLeague.Infrastructure --startup-project FootballLeague.API
- Package Manager Console: Update-Database -StartupProject FootballLeague.API (Default Project should be set to FootballLeague.Infrastructure)

3. **Run the API:**
-dotnet run --project FootballLeague.API --launch-profile https
- The API should be listening on port  https://localhost:7162, so you can open swagger with https://localhost:7162/swagger
- Second option is manually from VS, assure FootballLeague.API is configured as startup project and press F5 - a browser should open with https://localhost:7162/swagger/index.html

4. **Run tests:**
- run dotnet test from the root directory or manually from VS.

## Project Structure

- `FootballLeague.API` - API endpoints and startup
- `FootballLeague.Core` - Business logic and domain models
- `FootballLeague.Infrastructure` - Data access and migrations
- `FootballLeague.Tests` - Unit and integration tests
