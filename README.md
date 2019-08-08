# EasyQuery ASP.NET Core Samples

This repository contains several ASP.NET Core 2.x projects which demonstrate how to use [EasyQuery library](https://korzh.com/easyquery) in different web-application scenarios.

## EasyQuery packages

|NuGet Stable|NuGet Preview|NPM Latest|
|---|---|---|
|[![NuGet](https://img.shields.io/nuget/v/Korzh.EasyQuery.AspNetCore)](https://www.nuget.org/packages/Korzh.EasyQuery.AspNetCore)|[![NuGet](https://img.shields.io/nuget/vpre/Korzh.EasyQuery.AspNetCore)](https://www.nuget.org/packages/Korzh.EasyQuery.AspNetCore)|[![Npm](https://img.shields.io/npm/v/@easyquery/ui/latest)](https://www.npmjs.com/package/@easyquery/ui)|

## EasyQuery.JS browsers support

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>IE / Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera |
| --------- | --------- | --------- | --------- |
| IE11, Edge| last version| last version| last version


## Prerequisites

To run these samples you will need:

* [.NET Core 2.0](https://www.microsoft.com/net/core)
* [SQL Server Express LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) (it's installed automatically with Visual Studio)
* Node.js (to run Angular, Vue or React projects)
* Visual Studio 2017 or higher (optional)


## Getting started with Visual Studio 

 * Clone the repository
 * Open EqAspNetCoreSamples.sln solution file in Visual Studio
 * Build and run.
 
 
## Gettings started with the command-line

 * Clone the repository
 * Run Command Prompt and change the current directory to the project's folder for one of the demo projects (e.g. EasyReportDemo)
 * Type `dotnet build` to build the project
 * Type `dotnet run` to run it
 * Open `localhost:5000` in your browser.


## EqAspNetCoreDemo project

This project implements several of the most usual scenarios of using EasyQuery. We tried to combine all these cases in one application for two reasons:
 * to simplify the demonstration process since it's more easy to set up and run one project instead of several different projects.
 * to show how to configure different scenarios of using EasyQuery in one application.

So, when you start this sample project you will see an index page which leads you to the following demo pages:

### Advanced search

The page itself is implemented as a Razor Page (`Pages/AdvancedSearch.cshtml`). The scripts and CSS files are taken directly from our CDN and the initialization of the client-side code was done right in the `Scripts` section of .cshtml file.
The middleware for this scenario (the first `UseEasyQuery` call in `Startup.Configure`) is listening for requests on default endpoint `/api/easyquery`. The model is loaded from the XML file `App_Data/NWindSQL.xml` but you can easily switch to loading it directly from DB connection (just uncomment one line in middleware's configuration).

### Ad-hoc reporting

This is the page which demonstrates full capabilities of EasyQuery library: columns editing (with `ColumnsBar` widget), saving/loading of the queries to some server-side storage and loading the data model directly from a DbContext.
The page is available at `Views/Home/AdhocReporting.cshtml`. As for the JavaScript part - we use a different approach here. The code that we need to run this page is placed into a TypeScript file at `ts/adhoc-reporting.ts`. The necessary NPM packages (`@easyquery/core` and others) are listed in `package.json` file and everything is build with the help of WebPack using `npm run build` command. 
The result file (`adhoc-reporting.js`) is placed to `wwwroot/js` folder and then included in our view.


### Data filtering

To implement this scenario we used a totally different approach. The EasyQuery middleware and most of our client-side widgets are not used here. The page which is responsible for the implementation of this scenario is available at `Views/Order/Orders.cshtml`. The only widget added on that page is `FilterBar`. As in the previous case the necessary script (eq.all.min.js) and CSS file (eq.core.min.css) are bundled with WebPack and placed to `wwwroot` folders.
The server side part is implemented in `Controllers/OrderController.cs` file. Basically, in addition to `Index` action it contains only 3 extra methods which handles the requests from EasyQuery client-side code: `GetModel` (returns the model), `GetList` (returns the lists of values for lookup columns) and `ApplyQueryFilter` which executes the query (filter) over `Orders` DbSet using `DynamicQuery` extension method and passes the result lost of orders to `_OrderListPartial` partial view for rendering.

This is a great demonstration of using EasyQuery components without the middleware part.
 
### Full-text search

The last scenario is even simpler than the previous one. It demonstrates how quickly you can implement a full-text search over your database with only one useful extension function provided by EasyQuery: `FullTextSearchQuery`.
The view is implemented as a Razor page (`Pages/FullTextSearch`) and does not contain any EasyQuery JavaScript at all. All the magic happens on the code-behind class in `OnGet` method: we just call our `FullTextSearchQuery` function over the `Customers` DB set. 
On the page, we also use `eq-highlight-text` tag helper to highlight the found parts of the text inside the data table.

 
## Sample database

All of these demo projects work with some sample database. That database is created and initialized automatically at the first start. It may take some time (about 1-2 minutes) - so, please don't worry. Next time the app will be up and ready in a few seconds after launch.

The sample database is created in your SQL Express LocalDB instance by default. To change that you can modify the connection string in `appsettings.json` file in the project's folder.
 

## Links

 - [EasyQuery home page](https://korzh.com/easyquery)
 - [EasyQuery documentation](https://korzh.com/easyquery/docs)
 - [EasyQuery live demos](http://demo.easyquerybuilder.com)
 