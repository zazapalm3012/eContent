# eContentAppSolution

## Project Overview

This solution is a content management application built with .NET and ASP.NET Core, designed to manage categories, posts, and media. It follows a clean architecture to ensure separation of concerns and maintainability.

## Features

*   **Category Management**: Create, read, update, and delete content categories.
*   **Post Management**: Create, read, update, and delete posts with rich content and associated media.
*   **Media Management**: Upload and manage media files (images).
*   **API Endpoints**: RESTful APIs for interacting with the application's data.
*   **Web Frontend**: A web interface for content creation and display.

## Technologies Used

### Backend (eContentApp.Application, eContentApp.Domain, eContentApp.Infrastructure, eContentApp.Host)

*   **.NET 9.0**: The core framework for building the application.
*   **ASP.NET Core**: For building the web API and web frontend.
*   **Entity Framework Core**: ORM for database interaction.
*   **SQL Server (LocalDB)**: Default database for development.
*   **AutoMapper**: For object-to-object mapping.

### Frontend (eContentApp.Web)

*   **ASP.NET Core MVC**: For server-side rendering of views.
*   **jQuery**: JavaScript library for DOM manipulation and AJAX calls.
*   **Quill.js**: Rich text editor for post content.

### Testing

*   **xUnit**: Unit testing framework for .NET backend.
*   **Moq**: Mocking library for .NET unit tests.

## Project Structure

```
eContentAppSolution/
├── eContentApp.Application/  # Application services, DTOs, interfaces, and mapping
├── eContentApp.Domain/       # Domain entities and business logic
├── eContentApp.Host/         # Web API project (main entry point for API)
├── eContentApp.Infrastructure/ # Data access layer (EF Core, repositories)
├── eContentApp.Web/          # Web frontend project (MVC views, static files)
├── eContentApp.Tests/        # Unit and Integration Tests for backend and frontend
└── eContentAppSolution.sln   # Visual Studio Solution file
```

## Getting Started

### Prerequisites

*   [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
*   [Visual Studio](https://visualstudio.microsoft.com/downloads/) (Recommended IDE for .NET development)

### Installation

1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    cd eContentAppSolution
    ```

2.  **Restore .NET dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Database Setup (LocalDB):**
    The application uses SQL Server LocalDB by default. Ensure your LocalDB instance is running. Entity Framework Core migrations will apply automatically on startup if the database does not exist or is not up-to-date.

## Running the Application

### Running the API (eContentApp.Host)

Navigate to the `eContentApp.Host` directory and run:

```bash
cd eContentApp.Host
dotnet run
```

This will start the API server, usually on `http://localhost:5026` (check `launchSettings.json` for exact URL).

### Running the Web Frontend (eContentApp.Web)

Navigate to the `eContentApp.Web` directory and run:

```bash
cd eContentApp.Web
dotnet run
```

This will start the web application, usually on `http://localhost:5027` (check `launchSettings.json` for exact URL).

Alternatively, you can open `eContentAppSolution.sln` in Visual Studio and run the projects directly from there.

## Running Tests

All tests (backend C#) can be run from the solution root directory.

```bash
dotnet test
```

This command will discover and run all xUnit tests in `eContentApp.Tests` and Jest tests configured in `eContentApp.Web`.

### Running Specific Test Types

*   **Backend C# Tests only:**
    ```bash
    dotnet test eContentApp.Tests/eContentApp.Tests.csproj
    ```



