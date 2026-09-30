# Razor.AdvancedSearch

An ASP.NET Core Razor Pages sample that implements the **advanced search scenario**: a full-page query builder where users pick entities and columns, define conditions, run the query and work with the result — with a responsive layout that adapts to small screens.

What you will see on the page:

* **Entities** panel — the tree of all entities and attributes of the data model; drag or click to add them to the query.
* **Columns** panel — the result columns (with sorting and aggregate functions).
* **Conditions** panel — the query conditions, grouped with AND/OR.
* **Result** panel — the result grid and a chart (Chart.js). The grid supports **server-side multi-column sorting** (click a header), **column resizing** with widths remembered in `localStorage`, and paging (30 rows).
* **Full-text search** in the result header — the text is sent to the server and turned into a group of hidden conditions (see `UseQueryTuner` in `Services/EasyQueryConfigurator.cs`). Use `||` to search for several terms.
* **Export** — PDF, Excel and CSV, plus **Export Async** (Excel) that runs as a background job for large results.
* **Toolbar / sidebar** — New, Load, Clear, Save, Save As, Remove and Fetch Data. A set of predefined queries is included (`App_Data/dm-nwind/queries`).

All EasyQuery settings are in one class — `Services/EasyQueryConfigurator.cs` (an `IEasyQueryConfigurator` registered with `AddEasyQuery<EasyQueryConfigurator>()`); `app.MapEasyQuery(...)` in `Program.cs` is left empty and uses the default endpoint `/api/easyquery`.

## Tech info

| | |
|---|---|
| .NET | **.NET 10** (`net10.0`) |
| Platform | ASP.NET Core |
| UI framework | **Razor Pages** |
| EasyQuery for .NET | **7.5.0-rc04** (pre-release) — `Korzh.EasyQuery.AspNetCore`, `Korzh.EasyQuery.EntityFrameworkCore.Relational`, `Korzh.EasyQuery.DataExport`, `Korzh.EasyQuery.RazorUI`, `Korzh.EasyQuery.SqLiteGate`, `Korzh.EasyQuery.SqlServerGate` |
| EasyData exporters | 1.6.0-rc01 — `EasyData.Exporters.PdfSharp` (PDF), `EasyData.Exporters.ClosedXML` (Excel) |
| EasyQuery.JS | **7.5.0**, loaded from CDN (`eq.enterprise.min.js`, `eq.core.min.css`, `eq.view.min.css`); charts by Chart.js |
| Query engine | SQL (`EasyQueryManagerSql`) |
| Data model source | EF Core `DbContext` (`UseDbContext<AppDbContext>`); an XML model file (`App_Data/NWindSQL.xml`) is included as an alternative |
| EasyQuery API | Middleware — `app.MapEasyQuery()`, endpoint `/api/easyquery`, configured by `EasyQueryConfigurator` |
| Database | SQLite through EF Core 10 (SQL Server is prepared in commented code) |
| Query storage | `FileQueryStore` (XML files in `App_Data`), or `SessionQueryStore` — see below |

## Project structure

| File / folder | What's inside |
|---|---|
| `Program.cs` | Services (DbContext, session, EasyQuery + exporters + background jobs), `MapEasyQuery`, demo DB initialization |
| `Services/EasyQueryConfigurator.cs` | **All EasyQuery settings**: manager, model loader, query store, model tuner, full-text search query tuner, SQL formats |
| `Services/SessionQueryStore.cs` | A query store that keeps queries in the user's session, seeded from the files in `App_Data` |
| `CustomEasyQueryManagerSql.cs` | An (unused) manager subclass showing where to modify SQL before execution |
| `ValuesController.cs` | An example API (`/api/values/list1`) returning a custom value list |
| `Pages/Index.cshtml` | The page and EasyQuery.JS initialization (`AdvancedSearchView`) |
| `Pages/Shared/_Layout.cshtml` | The app bar with query commands and the sidebar |
| `Models/NWind/`, `Data/AppDbContext.cs` | Entity classes and the DbContext of the demo (Northwind) database |
| `Data/DbInitializeExtensions.cs` | Creates and seeds the database on the first run |
| `App_Data/dm-nwind/queries/` | Predefined queries for the `nwind` model |
| `App_Data/NWindSQL.xml` | A data model saved to an XML file (not used by default) |
| `App_Data/EqDemoData.zip` | Demo data used to seed the database |

## How to start

1. Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
2. Put your EasyQuery license key into `appsettings.json`:

   ```json
   "EasyQuery": {
     "LicenseKey": "<your key>"
   }
   ```

   or, better, into user secrets (run in the project folder):

   ```bash
   dotnet user-secrets set "EasyQuery:LicenseKey" "<your key>"
   ```

   You can get a trial key in the [Korzh.com Client's Area](https://account.korzh.com) after registering an account.

3. Run the project:

   ```bash
   dotnet run
   ```

   or open `../Samples.AspNetCore.sln` and start `EqDemo.AspNetCoreRazor.AdvancedSearch`.
4. Open https://localhost:5001 (or http://localhost:5000).

On the first run the application creates the SQLite database `eqdemo-sqlite.db` in the project folder and fills it with the demo data from `App_Data/EqDemoData.zip`. To reset the data, stop the app and delete the `eqdemo-sqlite.db*` files.

Try it: click **Load Query** and open one of the predefined queries, or build your own — add a few columns from the Entities panel, add a condition, press **Fetch Data**. Then click a column header to sort, or type something in the search box above the result.

## Next steps

Almost everything below is changed in `Services/EasyQueryConfigurator.cs`.

### Keep queries per browser session instead of shared files (configuration)

By default queries are saved as XML files in `App_Data/dm-nwind/queries`, shared by everyone. Set `QueryStoreMode` to `session` and each browser session gets its own copy of the predefined queries (changes are lost when the session ends — good for a public demo):

```json
"QueryStoreMode": "session"
```

in `appsettings.json`, or as an environment variable — `Program.cs` reads the variables with the `AdvancedSearch_` prefix:

```bash
AdvancedSearch_QueryStoreMode=session
```

To store the files as JSON instead of XML, change `FileFormat = "xml"` to `"json"` in the `FileQueryStoreSettings`.

### Switch to SQL Server (uncomment + change)

1. `appsettings.json` — the app reads the connection string named `EqDemoDbLite` (in both `Program.cs` and `EasyQueryConfigurator.cs`). Put your SQL Server connection string there (the `_alt_EqDemoDb` entry has an example), or rename the key in both places.
2. `Program.cs` — replace `options.UseSqlite(dbConnectionString)` with the commented `options.UseSqlServer(...)` line, and register the SQL Server gate instead of the SQLite one: `.RegisterDbGate<Korzh.EasyQuery.DbGates.SqlServerGate>();` (the commented line below it).
3. `Data/DbInitializeExtensions.cs` — replace `options.UseSqlite(connectionString)` with the commented `options.UseSqlServer(...)`.
4. `EasyQueryConfigurator.cs` — change `options.UseSqlFormats(FormatType.Sqlite, ...)` to `FormatType.MsSqlServer` (or remove the call — the formats are otherwise taken from the DbContext provider).

### Load the model from the XML file

`App_Data/NWindSQL.xml` is a data model saved to a file (it's copied to the output folder by the `.csproj`). To use it instead of the DbContext, replace `options.UseDbContext<AppDbContext>();` with:

```csharp
using Microsoft.Data.Sqlite;
...
options.DefaultModelId = "NWindSQL";                                  // FileModelLoader looks for App_Data/<model ID>.xml or .json
options.UseModelLoader(_ => new FileModelLoader("App_Data"));
options.UseDbConnection<SqliteConnection>();                          // uses options.ConnectionString set above
```

Things to adjust together with it:

* The model ID becomes the folder name for saved queries — rename `App_Data/dm-nwind` to `App_Data/dm-NWindSQL` (or rename the XML file to `nwind.xml` and keep `DefaultModelId = "nwind"`).
* Saved queries and the model tuner (`UseModelTuner`) refer to entities and attributes by their IDs (`Order.ShipRegion`, `Category.CategoryName`, `Product.Category`). If the file model uses different IDs, `FindEntityAttr` returns `null` — update or remove those lines.

### Load the model directly from the database schema

The SQLite gate is already registered in `Program.cs` (`.RegisterDbGate<SqLiteGate>()`), so you only need to replace `options.UseDbContext<AppDbContext>();` with:

```csharp
options.UseDbConnection<SqliteConnection>();
options.UseDbConnectionModelLoader(loaderOptions => {
    loaderOptions.IgnoreViews();
    // loaderOptions.AddTableFilter(...), UseSchemas("dbo"), DoNotPrettifyNames(), ...
});
```

The same note about `UseModelTuner` and the saved queries applies: the IDs in a schema-based model may differ.

### Add hidden conditions to every query

The query tuner (`options.UseQueryTuner(...)`) runs for each request and already adds the full-text search conditions to `query.ExtraConditions`. Add your own conditions there — after `Conditions.Clear()` — to restrict the data for everyone, invisibly to users:

```csharp
options.UseQueryTuner(manager => {
    var query = manager.Query;
    query.ExtraConditions.Conditions.Clear();

    query.ExtraConditions.AddSimpleCondition("Order.ShipCountry", "Equal", "USA");   // always applied

    // ...the full-text search part stays below
});
```

### Change the full-text search

In the same query tuner, `DbFullTextSearchOptions` controls what is searched:

* `IncludeDateTimeFields`, `IncludeNumericFields` — search in dates and numbers too (both `true` in the sample);
* `Entities` / `IncludeRootEntity` — the default scope when the query has no columns or conditions.

The search scope is otherwise taken from the query itself: its result columns, or the entities used in its conditions.

### Fill the "Ship Region" list

The model tuner gives `Order.ShipRegion` a custom list editor (`CustomListValueEditor("Lookup", "Lookup")`), but nothing provides values for the `Lookup` list yet, so it's empty. Provide them on the server:

```csharp
options.AddValueListResolver(request => {
    if (request.ListName == "Lookup") {
        return new List<ListItem> {
            new ListItem("WA", "Washington"),
            new ListItem("OR", "Oregon")
        };
    }
    return null;   // not our list - let other resolvers handle it
});
```

or on the client, with the `onListRequest` handler in `Pages/Index.cshtml` — e.g. from the `/api/values/list1` endpoint of `ValuesController`:

```js
onListRequest: function (params, onResult) {
    if (params.editorId === 'Lookup') {
        fetch('/api/values/list1').then(r => r.json()).then(onResult);
        return true;    // handled here, don't ask the EasyQuery API
    }
    return false;
},
```

### Modify SQL before execution

`CustomEasyQueryManagerSql.cs` overrides `PrepareDbCommand` — the place to adjust `statement.SQL` (add hints, rewrite table names, log it). It's not wired in; to use it, change the first line of `Configure`:

```csharp
options.UseManager<CustomEasyQueryManagerSql>();   // instead of UseManager<EasyQueryManagerSql>()
```

### Change the page behavior

In `Pages/Index.cshtml`, `viewOptions` of the `AdvancedSearchView`:

* `loadQueryOnStart: true` plus `defaultQueryId: '<query id>'` — open the page with one of the saved queries.
* `const showPanelButtons = false` — set to `true` to always show the buttons in the Columns and Conditions panels.
* `widgets.queryPanel.minDate` / `maxDate` (commented) — limit the date pickers, e.g. `minDate: '2025-01-01', maxDate: 'today'`.
* `locale` (commented) and `localeSettings.shortDateFormat` — the date format and UI language.
* `widgets.resultGrid` — `sortable`, `allowColumnResize`, and `columnWidthPersistence` (`scope: 'query'` keeps separate column widths for each saved query).
* `result.showChart`, `result.paging.pageSize` — the chart and the page size.
* `serverExporters` and the export buttons in the markup — the export formats (`pdf` and `excel` must be registered with `.AddDataExporter<...>()` in `Program.cs`).

### The built-in Advanced Search page and its settings (uncomment)

`Program.cs` sets `Korzh.EasyQuery.RazorUI.Pages.AdvancedSearch.ExportFormats` and has a commented `...ShowSqlPanel = true;` line. These static settings belong to the ready-made page that the `Korzh.EasyQuery.RazorUI` package adds to the app at `/EasyQuery/AdvancedSearch` — not to this sample's own `Pages/Index.cshtml`, which has its own markup. Uncomment the line and open `/EasyQuery/AdvancedSearch` to see the generated SQL under the result.
