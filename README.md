# MVC Assignments

Eleven small web applications built with **ASP.NET Core MVC** (.NET 11). Every folder is an
independent project: open it on its own and run `dotnet run`.

| # | Folder | Description |
|---|--------|-------------|
| 01 | `01-TodoList` | Web app to add, delete and mark tasks as completed. |
| 02 | `02-TipCalculator` | Calculates the tip from the total bill and the selected tip percentage. |
| 03 | `03-PasswordGenerator` | Generates random secure passwords with adjustable length and character types. |
| 04 | `04-ExpenseTracker` | Record daily expenses, categorize them and see a monthly summary. |
| 05 | `05-BookingSystem` | Book appointments or services online, see availability and confirm the booking. |
| 06 | `06-NotesManager` | Write and save notes, organize them in categories and search them. |
| 07 | `07-EventCalendar` | Interactive monthly calendar to add, edit and delete events, with reminders and notifications. |
| 08 | `08-RecipePlatform` | Search, save and share cooking recipes. Filter by meal type and ingredients. |
| 09 | `09-MemoryGame` | Classic memory card game: match pairs of cards, three difficulty levels and a high-score table. |
| 10 | `10-SurveyPlatform` | Create and answer online surveys, then analyze the results with visual reports. |
| 11 | `11-Stopwatch` | Online stopwatch with start, pause, reset and lap / split time recording (laps are saved in the database). |

Projects that store data use **Entity Framework Core + SQLite**; the `app.db` file is created
automatically the first time the project runs, so no extra setup is needed.
