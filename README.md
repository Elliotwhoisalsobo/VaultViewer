<p align="center">
  <img src="VaultViewer/VaultViewer/UI/Pictures/icon.png" alt="VaultViewer-logo" width="399">
</p>

<h1 align="center">VaultViewer</h1>

VaultViewer is a WPF desktop application for managing role-based access to employee, customer, product, and login data stored in MySQL.

It was built to replace the manual process of requesting sensitive data from multiple managers with a central, access-controlled application that fits restrictive security policies.
It implements a layered architecture.

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

## Running from source

Requirements:

- Windows
- .NET 8 SDK
- MySQL Server

1. Create the database using `Database_script.sql`.
2. Optionally load `example_data.sql`.
3. Update the `VaultViewer` connection string in `VaultViewer/VaultViewer/DataAccessLayer/App.config`.
4. Run the application:

```powershell
dotnet run --project VaultViewer/VaultViewer/VaultViewer.csproj
```

`dotnet run` restores the required NuGet packages automatically.

## Running a release

A self-contained Windows release does not require the .NET SDK or a package restore. Extract the release archive and start `VaultViewer.exe`. A configured MySQL database is still required.

To create the release archive:

```powershell
dotnet publish VaultViewer/VaultViewer/VaultViewer.csproj -c Release -r win-x64 --self-contained true -o publish
Compress-Archive -Path publish\* -DestinationPath VaultViewer-win-x64.zip
```

## Technology

- C# and .NET 8
- WPF
- MySQL
- ClosedXML
- BCrypt.Net
