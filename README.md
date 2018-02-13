# EasyQuery ASP.NET Core Samples
ASP.NET Core 2.0 projects which demonstrate how to use EasyQuery in different web-application scenarios.

## Prerequisites
To run these samples you will need:
 * [.NET Core 2.0](https://www.microsoft.com/net/core)
 * [SQL Server Express LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) (it's installed automatically with Visual Studio)
 * Visual Studio 2017 (optional)
 

## Getting started with Visual Studio 
 * Clone the repository
 * Open EqAspNetCoreSamples.sln solution file in Visual Studio
 * Build and run.
 
 
## Gettings started with command-line
 * Clone the repository
 * Run Command Prompt and change current directory to the project's folder for one of the demo projects (e.g. EasyReportDemo)
 * Type `dotnet build` to build the project
 * Type `dotnet run` to run it
 * Open `localhost:5000` in your browser.

 
 
## Sample database
All of these demo projects work with some sample database. That database is created and initialized automatically at the first start. It may take some time (about 1-2 minutes) - so, please don't worry. Next time the app will be up and ready in a few seconds after launch.

The sample database is created in your SQL Express LocalDB instance by default. To change that you can modify the connection string in `appsettings.json` file in the project's folder.
 
