# EasyQuery ASP.NET Core Samples

This repository contains several ASP.NET Core 2.x and 3.0 projects which demonstrate how to use [EasyQuery library](https://korzh.com/easyquery) in different web-application scenarios.

## EasyQuery packages

|NuGet Stable|NuGet Preview|NPM Latest|
|---|---|---|
|[![NuGet](https://img.shields.io/nuget/v/Korzh.EasyQuery.AspNetCore)](https://www.nuget.org/packages/Korzh.EasyQuery.AspNetCore)|[![NuGet](https://img.shields.io/nuget/vpre/Korzh.EasyQuery.AspNetCore)](https://www.nuget.org/packages/Korzh.EasyQuery.AspNetCore)|[![Npm](https://img.shields.io/npm/v/@easyquery/ui/latest)](https://www.npmjs.com/package/@easyquery/ui)|

## EasyQuery.JS browsers support

| [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/edge/edge_48x48.png" alt="IE / Edge" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>IE / Edge | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/firefox/firefox_48x48.png" alt="Firefox" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Firefox | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/chrome/chrome_48x48.png" alt="Chrome" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Chrome | [<img src="https://raw.githubusercontent.com/alrra/browser-logos/master/src/opera/opera_48x48.png" alt="Opera" width="24px" height="24px" />](http://godban.github.io/browsers-support-badges/)</br>Opera | ![Without jQuery](https://i.ibb.co/ZKSGMjt/no-jquery-logo.jpg)
| --------- | --------- | --------- | --------- | --------- |
| IE11, Edge| last version| last version| last version | without jQuery |


## Prerequisites

To run these samples you will need:

* [.NET Core 2.0](https://www.microsoft.com/net/core)
* [SQL Server Express LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express) (it's installed automatically with Visual Studio)
* Node.js (to run Angular, Vue or React projects)
* Visual Studio 2017 or higher (optional)

## Getting started with Visual Studio 

* Clone the repository
* Open one of Samples.XXXX.sln files by your choice in Visual Studio (XXXX here defines the group of samples you may be interested in: `AspNet4`, `AspNetCore`, `WinForms` or `Wpf`)
* Build and run.

## Gettings started with the command-line

* Clone the repository
* Run Command Prompt and change the current directory to the project's folder for one of the demo projects (e.g. `AspNetCore/Razor-Mvc/Razor.AdHocReporting `)
* Type `dotnet run` to run the project
* Open `localhost:5000` in your browser.


## EasyQuery trial license key

At the first run you will be asked for a product key for EasyQuery. You can [register on our website](https://korzh.com/easyquery/get-started) to get the trial product key (works for 30 days). The registration takes about 1 minute. When you get the key, add it to your appsettings.json file (for ASP.NET Core project), to web.config (for ASP.NET 4) or right into the code (for WinForms or WPF). Here are the examples:

### ASP.NET Core (appsettings.json)

```
{
   .    .    .    .  
  "EasyQuery": {
    "LicenseKey": "Your trial version key goes here"
  }
}
```

### ASP.NET (web.config)

```
<configuration>
  .   .   .   .  
  <appSettings>
    .   .   .   .  
    <add key="EasyQuery:LicenseKey" value="Your trial version key goes here" />
    .   .   .   .
```

### WPF 

In the constructor of the page:

```
Korzh.EasyQuery.Wpf.License.Key = "Your trial version key goes here";
```

### WPF 

In the constructor of the form:

```
Korzh.EasyQuery.WinForms.License.Key = "Your trial version key goes here";
```


## Repository structure

All sample project in the repository are divided by EasyQuery editions (ASP.NET Core, ASP.NET 4, WPF and Windows Forms) and then by frameworks and specific scenarios.

For example, inside `AspNet4` folder you will find 2 sub-folders: `WebForms` and `MVC` with the samples, correspondingly for WebForms and MVC frameworks. `AspNetCore` folder contains the following sub-folders: `Razor-MVC`, `Angular`, `React`, `Vue`, `Stencil`. The name of each folder is self-explanatory.

Finally, on the last level the sample projects are divided by particular scenarios. There are 3 basic scenarios that we support: __Advanced Search__, __Ad-hoc Reporting__ and __Data Filtering__. Let's look at them in more detail:

### Advanced search

This is the most popular scenario. It's implemented in all editions and for the most of supported frontend frameworks. This scenario will be very helpful when your users need to perform a data search by many different parameters and in various combinations.

### Ad-hoc reporting

With this scenario you get a basic implementation of an ad-hoc reporting solution. Users can manage the list of reports, setup the columns and conditions for each report and define the way they would like to see the result data. In this scenario we demonstrate full capabilities of EasyQuery framework. Not only as a decent query builer but also as a simple BI tool. The result data for each report can be represented in a simple tabular form (with aggregation, grouping, sub-totals, grand-totals, if necessary), as a chart or a pivot table.

### Data filtering + text search

In this scenario we demonstrate how to use EasyQuery components without the middleware (or Web API controller) part. 
It's the most simple way to add data filtering and paging functionality to the existing pages that show the data for one database entity (table). 
You just add some new HTML elements on your page and a few pieces of code to your controller (or code-behind class in case of Razor pages). 
This works especially well with standard CRUD forms created with Visual Studio scaffolding tool.

And with a few extra-lines of code and markup this kind of sample project can show you how to add a simple text search functionality for your data page. This scenario is described in the following [YouTube video](https://www.youtube.com/watch?v=0XQT6x0Ge08). 


## Sample database

All of these demo projects work with some sample database. That database is created and initialized automatically at the first start. It may take some time (about 1-2 minutes) - so, please don't worry. Next time the app will be up and ready in a few seconds after launch.

The sample database is created in your SQL Express LocalDB instance by default. To change that you can modify the connection string in `appsettings.json` file in the project's folder.

## Links

* [EasyQuery home page](https://korzh.com/easyquery)
* [EasyQuery documentation](https://korzh.com/easyquery/docs)
* [EasyQuery live demo](http://korzh.com/demo)
