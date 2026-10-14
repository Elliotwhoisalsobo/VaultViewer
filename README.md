# VaultViewer

VaultViewer is a WPF desktop application for managing role-based access to employee, customer, product, and login data stored in MySQL.

It was built to replace the manual process of requesting sensitive data from multiple managers with a central, access-controlled application that fits restrictive security policies.
IT fully implements a layered-architecture design pattern.

## Features

- BCrypt password authentication
- Role-based access to user, HR, engineering, and administration data
- Employee creation, soft deletion, and role assignment
- Configurable row limits
- CSV and Excel export

## Architecture

The application uses four layers:

```text
UI → Service layer → Data access layer → MySQL
              ↘ Business models ↗
```

- `Business` contains the domain models.
- `DataAccessLayer` contains MySQL repositories and connection management.
- `ServiceLayer` contains authentication and application workflows.
- `UI` contains the WPF windows and event handlers.

## Requirements

- Windows
- .NET 8 SDK
- MySQL Server

## Setup

1. Create the database using `Database_script.sql`.
2. Optionally load `example_data.sql`.
3. Update the `VaultViewer` connection string in `VaultViewer/VaultViewer/DataAccessLayer/App.config`.
4. Build and run the application:

```powershell
dotnet restore VaultViewer/VaultViewer.sln
dotnet run --project VaultViewer/VaultViewer/VaultViewer.csproj
```

## Technology

- C# and .NET 8
- WPF
- MySQL
- ClosedXML
- BCrypt.Net
