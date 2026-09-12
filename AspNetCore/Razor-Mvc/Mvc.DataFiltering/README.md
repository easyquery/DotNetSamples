# Mvc.DataFiltering

An ASP.NET Core MVC sample that implements the **data-filtering scenario**: a regular list page (Orders) gets a filter bar where users build their own filtering conditions, plus a full-text search box.

What you will see on the page (`/data-filtering`):

* **FilterBar** — an EasyQuery widget where users add conditions like "Customer country is USA" or "Order date is in the last year". Pressing **Apply** re-fetches the list.
* **Text Search** — a plain text box. The text is sent to the server with each fetch request and applied as a full-text search over a few selected fields (customer name/country, employee first/last name).
* **Result list** — rendered **on the server** as a Razor partial view (`_OrderListPartial.cshtml`) with paging (15 rows per page) and highlighting of the searched text. EasyQuery.JS only builds the filter; the markup of the list is yours.
* **Save Query / Load Query** — saves the current filter under the ID `LastQuery` and loads it back.

Unlike most other samples, this one does **not** use the EasyQuery middleware (`MapEasyQuery`). All the EasyQuery API endpoints are ordinary MVC actions in `OrderController`, which creates an `EasyQueryManagerLinq<Order>` by itself. This is the approach to take when you need full control over each request, or want to return your own views instead of JSON.

## Tech info

| | |
|---|---|
| .NET | .NET 8 (`net8.0`) |
| Platform | ASP.NET Core |
| UI framework | **MVC** (controllers + Razor views) |
| EasyQuery for .NET | **7.4.2** — `Korzh.EasyQuery.AspNetCore`, `Korzh.EasyQuery.Linq` |
| EasyQuery.JS | **7.4.2**, loaded from CDN (`eq.enterprise.min.js`, `eq.core.min.css`) |
| Query engine | LINQ — filters are applied to an `IQueryable<Order>` via `DynamicQuery()` |
| Data model source | The `Order` class and its navigation properties (`UseEntity`) |
| Database | SQLite through EF Core 8 (SQL Server is prepared in commented code) |
| Query storage | `FileQueryStore` in the `App_Data` folder |

## Project structure

| File / folder | What's inside |
|---|---|
| `Program.cs` | Service registration (DbContext, session, `AddEasyQuery()`), MVC routing, demo DB initialization |
| `Controllers/OrderController.cs` | The EasyQuery endpoints under `/data-filtering`: get model, get value list, load/save query, fetch (apply filter) |
| `Controllers/HomeController.cs` | `/` simply redirects to the Orders page |
| `Views/Order/Orders.cshtml` | The page: FilterBar, text search, Load/Save buttons, EasyQuery.JS initialization (`HtmlDataFilterView`) |
| `Views/Order/_OrderListPartial.cshtml` | The server-rendered result list (`<eq-highlight-text>`, `<eq-page-navigator>` tag helpers) |
| `Models/NWind/` | Entity classes of the demo (Northwind) database |
| `Data/AppDbContext.cs` | EF Core DbContext |
| `Data/DbInitializeExtensions.cs` | Creates and seeds the database on the first run |
| `App_Data/EqDemoData.zip` | Demo data used to seed the database |

## How to start

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or later).
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

   > **NB:** the page passes `Korzh.EasyQuery.AspNetCore.JSLicense.Key` to `useEnterprise(...)`. If your EasyQuery.JS key is separate from the .NET one, add it as `EasyQuery:JSLicenseKey` as well. Without a valid EasyQuery.JS key the **Save / Load Query** buttons silently work with the browser's local storage instead of the server's `FileQueryStore`.

3. Run the project:

   ```bash
   dotnet run
   ```

   or open `../Samples.AspNetCore.sln` in Visual Studio / Rider and start `EqDemo.AspNetCoreMvc.DataFiltering`.
4. Open https://localhost:5001 (or http://localhost:5000).

On the first run the application creates the SQLite database file `eqdemo-sqlite.db` in the project folder and fills it with the demo data from `App_Data/EqDemoData.zip`. To reset the data, stop the app and delete the `eqdemo-sqlite.db*` files.

Try it: add a condition in the filter bar (e.g. *Customer → Country is equal to USA*), press **Apply**, then type a name in **Text Search** and press **Search**. Both filters are combined.

## Next steps

Below are the most common things to change and where to do it.

### Switch to SQL Server (uncomment)

The SQL Server variant is already in the code, commented out:

1. `appsettings.json` — comment the SQLite `EqDemoDb` connection string and uncomment the `(localdb)\\MSSQLLocalDB` one (or put your own).
2. `Program.cs` — replace `options.UseSqlite(...)` with the commented `options.UseSqlServer(...)` line.
3. `Data/DbInitializeExtensions.cs` — do the same in the `DbInitializer.Create(...)` block, so the demo data is seeded into SQL Server.

The `Microsoft.EntityFrameworkCore.SqlServer` and `Korzh.DbUtils.SqlServer` packages are already referenced.

### Filter a different entity or include more related data

The data model is built from the entity type returned by `options.UseEntity(...)` in the `OrderController` constructor. Every navigation property you `Include` becomes available in the filter bar:

```csharp
options.UseEntity(_ =>
    _dbContext.Orders
        .Include(o => o.Customer)
        .Include(o => o.Employee)
        .Include(o => o.Items)              // add order lines to the model
        .AsQueryable(),
    loaderOptions => {
        loaderOptions.SortAlpabetically = false;   // keep the properties in declaration order
    });
```

Remember to add the same `Include` calls to the `IQueryable` built in `ApplyQueryFilter`, and update `_OrderListPartial.cshtml` if you want to display the new data. To filter another list (e.g. Customers), change the generic type of `EasyQueryManagerLinq<T>` and the `UseEntity`/`ApplyQueryFilter` queries accordingly.

### Tune how properties appear in the filter

The loader reads the attributes on the model classes (`Models/NWind/*.cs`):

* `[Display(Name = "...")]` — the caption shown to users (e.g. `Order.OrderDate` is shown as "Ordered").
* `[EqListValueEditor]` — shows a list of values for the property instead of a text box (see `Employee.Country`).
* `[EqEntityAttr(UseInConditions = false)]` — hides a property from the filter; `[EqEntityAttr(false)]` excludes it from the model completely. `[EqEntity(...)]` does the same for a whole class.

### Add a fixed (hidden) filter

Since the controller owns the `IQueryable`, any regular LINQ condition added in `ApplyQueryFilter` is combined with the user's filter and is never visible in the UI — handy for multi-tenant or "only my records" restrictions:

```csharp
var orders = _dbContext.Orders
          .Include(o => o.Customer)
          .Include(o => o.Employee)
          .Where(o => o.ShipCountry == "USA")      // always applied
          .AsQueryable();
```

### Open the page with a predefined filter

To restore the last saved query automatically when the page opens, add two options to `viewOptions` in `Views/Order/Orders.cshtml` (this is exactly what `Mvc.DataFiltering.Auth` does):

```js
var viewOptions = {
    loadModelOnStart: true,
    loadQueryOnStart: true,       // load a query right after the model
    defaultQueryId: 'LastQuery',  // ...with this ID
    ...
};
```

To ship a default filter, save one with the **Save Query** button and keep the resulting file from `App_Data` (queries are stored in a `dm-<model ID>/queries` sub-folder).

### Change the full-text search

`FullTextSearchOptions` in `ApplyQueryFilter` decides which fields are searched:

* `Filter` — a predicate over `PropertyInfo`. Currently it allows `Order.Customer`, `Order.Employee` and a few string fields of those classes. Return `true` for any other property you want to search in.
* `Depth` — how deep to follow navigation properties (2 by default).
* `OrderBy` / `IsDescendingOrder` — sort the result by some property.

### Change paging or the result markup

* Page size — the second argument of `orders.ToPagedList(_eqManager.Chunk.Page, 15)`.
* Columns and layout — `Views/Order/_OrderListPartial.cshtml` is a regular Razor view; `<eq-highlight-text>` highlights the search text in a value.

### Store queries somewhere else

`options.UseQueryStore(...)` in the controller defines the storage for Save/Load Query:

* XML files instead of JSON: `new FileQueryStore(new FileQueryStoreSettings { DataPath = "App_Data", FileFormat = "xml" })`.
* A database table, per user — implement `IQueryStore`; see `Services/DbQueryStore.cs` in the [Mvc.DataFiltering.Auth](../Mvc.DataFiltering.Auth/README.md) sample.

### Use the EasyQuery middleware instead of a controller

If you don't need a server-rendered result list, the whole `OrderController` can be replaced with one `app.MapEasyQuery(...)` call and EasyQuery.JS's own result grid. See the [Razor.DataFiltering](../Razor.DataFiltering/README.md) sample for this approach.
