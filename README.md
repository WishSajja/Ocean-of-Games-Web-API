# Ocean of Games API

A RESTful Web API powering **Ocean of Games**, a game discovery platform built with ASP.NET Core and Entity Framework Core.

The API integrates with the GameBrain API to retrieve game information and uses SQL Server to cache game data, enabling efficient retrieval and filtering.

## 🚀 Technologies Used

- **C# / ASP.NET Core Web API** – Backend development
- **Entity Framework Core** – Object-relational mapping (ORM)
- **Microsoft SQL Server** – Database management
- **GameBrain API** – External game information provider
- **LINQ** – Data querying and filtering

## ✨ Features

- Retrieve game information through RESTful endpoints.
- Search for games by name.
- Filter games by genre and platform.
- Sort games according to supported criteria.
- Integrate with the GameBrain API to retrieve external game information.
- Cache retrieved game data in SQL Server to reduce repeated external API requests.
- Use Entity Framework Core for database operations.

## 🏗️ Project Structure

```text
GameAPI/
├── Controllers/
├── Models/
├── Services/
├── Data/
├── Migrations/
├── Program.cs
├── appsettings.json
├── GameAPI.csproj
└── .gitignore
```

## ⚙️ Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET 9 SDK
- Microsoft SQL Server
- Visual Studio 2022 or a compatible IDE
- GameBrain API key

### 1. Clone the repository

```bash
git clone YOUR_REPOSITORY_URL
```

### 2. Navigate to the project

```bash
cd GameAPI
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Configure User Secrets

Initialize .NET User Secrets:

```bash
dotnet user-secrets init
```

Configure your database connection string:

```bash
dotnet user-secrets set "ConnectionStrings:GameDatabase" "YOUR_CONNECTION_STRING"
```

Configure your GameBrain API key:

```bash
dotnet user-secrets set "GAMEBRAIN_API_KEY" "YOUR_GAME
