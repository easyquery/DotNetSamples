# EasyQuery WebForms demo

This project demostrates how to use EasyQuery library (together with EasyQuery.JS scripts) in a WebForms application.

## How it works

### 1. The demo page

`EasyQuery.aspx` page is not actually a classic WebForms page.
It's just an HTML page which define several slot (empty `<div>` elements) for EasyQuery widgets and attach EasyQuery CSS file and scripts.
On the page start the script initialize EasyQuery's `view` object which do all the job.

### 2. Web API Controller

We use WebAPI packages to create an API controller which handles all AJAX requests from the client-side widgets.
You can it in `Controllers/AdvancedSearchController.cs` file.

