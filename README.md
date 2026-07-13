# Country API
UsedCore Web API for retrieving and caching country data from a public API.

Prerequisites
.NET 8 SDK
SQL Server
A SQL Server management tool (SQL Server Management Studio, Azure Data Studio, etc.)
Visual Studio Code or Visual Studio

Build and Run
1. Database Setup

Open the database script located at:

Country.Db/CountryCacheDBScript.sql
Copy the SQL script.
Open your preferred SQL Server management tool (such as SQL Server Management Studio or Azure Data Studio), execute the script, and create the database and the Countries table.
2. Open the Project

Open the project using either:

Visual Studio Code
Visual Studio

Note: Country.Api is the ASP.NET Core Web API project.

3. Run Using Visual Studio
Open the solution.
Restore the NuGet packages (if required)
Build the solution.
Run the application.
Swagger UI will open automatically in your browser.

4. Run Using Visual Studio Code
Open a terminal.
Navigate to the Country.Api project directory.

Restore NuGet packages:
- dotnet restore

Build the application:
- dotnet build

Run the application:
- dotnet watch
Once the application starts, Swagger UI will open automatically in your default browser.

Libraries 

1. Newtonsoft.Json : Used to deserialize the country data returned by the public API.
2. AutoMapper : Used to simplify object mapping between different models.
3. Microsoft.Data.SqlClient: Used to connect to SQL Server and perform database operations.
4. Swashbuckle.AspNetCore: Used to provide Swagger UI for testing the API endpoints.