# Mvc.DataFiltering.Auth

An ASP.NET Core (.NET 8) MVC project that implements the [data-filtering scenario](https://korzh.com/demo/easyquery-asp-net-core-razor/data-filtering) for a multi-user application with authentication.

It extends the basic `Mvc.DataFiltering` sample with:

* __ASP.NET Core Identity__ with register/login pages. Users and roles are stored in the same database as the demo data (via `IdentityDbContext`).
* __DbQueryStore__ — a custom implementation of `IQueryStore` that saves queries to the database and binds each of them to the current user's ID. Different users can store their queries under the same query ID (e.g. "LastQuery") — the `UserQueries` table uses a composite primary key (`Id` + `OwnerId`).
* __Automatic query loading__ — the filtering page is configured with `loadQueryOnStart: true` and `defaultQueryId: 'LastQuery'`, so the last query saved by the current user is loaded (and applied) automatically when the page opens.

## Project structure

| Folder / file | Description |
|---|---|
| `Program.cs` | Service registration (DbContext, Identity, EasyQuery) and the request pipeline |
| `Controllers/OrderController.cs` | The main controller. Defines EasyQuery endpoints (`/data-filtering/...`) for model loading, query load/save and data fetching. Marked with `[Authorize]` |
| `Services/DbQueryStore.cs` | The query store that keeps users' queries in the database |
| `Models/UserQuery.cs` | The entity that represents a stored query (with `OwnerId` pointing to the Identity user) |
| `Models/NWind/` | Entity classes of the demo (Northwind) database |
| `Data/AppDbContext.cs` | The DbContext. Inherits `IdentityDbContext`, so it also contains all Identity (users/roles) tables |
| `Data/DbInitializeExtensions.cs` | Creates and seeds the database on the first run: demo data, the `eq-manager` role and the default user |
| `Areas/Identity/` | Login, Register and Logout pages (Razor Pages, used by Identity) |
| `Views/Order/Orders.cshtml` | The main page with the FilterBar widget, Load Query / Save Query buttons and the result grid |
| `Views/Shared/_LoginPartial.cshtml` | The login/logout section of the navbar |
| `App_Data/EqDemoData.zip` | Demo data used to seed the database on the first run |

## How to run

1. Make sure [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or later) is installed.
2. Set your EasyQuery license key in `appsettings.json`:

   ```json
   "EasyQuery": {
     "LicenseKey": "<your key>"
   }
   ```

   or, better, put it into user secrets:

   ```
   dotnet user-secrets set "EasyQuery:LicenseKey" "<your key>"
   ```

   > __NB:__ without a valid license key, EasyQuery.JS falls back to storing queries in the browser's local storage, so `DbQueryStore` will not be called on Save/Load Query operations.

3. Run the project:

   ```
   dotnet run
   ```

   or launch it from Visual Studio. Then open https://localhost:5003 (or http://localhost:5002) in your browser.

On the first run the application creates the SQLite database (`eqdemo-auth-sqlite.db`), seeds it with the demo data and adds the default user:

* login: __demo@korzh.com__
* password: __demo__

You can also register your own account — every user gets their own set of saved queries.

To try the scenario: log in, define some filtering conditions in the FilterBar, press __Apply__ and then __Save Query__. After reloading the page (or logging in again later), the saved query is loaded and applied automatically.

To reset the database, stop the application and delete the `eqdemo-auth-sqlite.db*` files.

To switch to SQL Server, change the connection string in `appsettings.json` and replace `UseSqlite` with `UseSqlServer` in `Program.cs` and `Data/DbInitializeExtensions.cs`.
