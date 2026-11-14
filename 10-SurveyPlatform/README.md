# 10 - SurveyPlatform

Create and answer online surveys, then analyze the results with visual reports.

Built with **ASP.NET Core MVC** (.NET 8), following the Model-View-Controller pattern:

- `Models/` - data classes, validation rules and view models
- `Views/` - Razor views (Bootstrap UI)
- `Controllers/` - request handling and application logic

## Requirements

- .NET 11 SDK (https://dotnet.microsoft.com/download)

## How to run

```bash
cd 10-SurveyPlatform
dotnet run
```

Then open the URL printed in the console (for example `http://localhost:5000`).
If you use a different .NET version, change `<TargetFramework>` in the `.csproj` file
(for example `net8.0`). `UseAppHost` is disabled so Windows App Control does not block the
generated `.exe`; the app runs through `dotnet`.

If the project uses a database, a SQLite file (`app.db`) is created automatically on first run.
