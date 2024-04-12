Asset Tracking Application
Overview
The Asset Tracking Application is a console-based program developed in C# using Entity Framework for managing and tracking assets within an organization. It allows users to perform CRUD (Create, Read, Update, Delete) operations on assets stored in a database, as well as generate reports to analyze asset data.

Features
Add new assets with details such as type, brand, model, office, purchase date, currency, and price.
View a list of all assets stored in the database.
Update existing assets with new information.
Delete assets from the database.
Generate an Asset Summary Report to view the total number of assets, total value of assets (in USD), and average value of assets per office.
Getting Started
Clone the repository to your local machine.
Open the solution file (AssetTrackingV2.sln) in Visual Studio.
Build the solution to restore dependencies.
Modify the connection string in MyDbContext.cs to point to your desired database server.
Run the application.
Usage
Upon running the application, you will be presented with a menu with options to perform various actions.
Use the provided menu options to add, view, update, or delete assets.
Press "L" in the menu to generate the Asset Summary Report.
Requirements
.NET Core SDK (version X.X.X)
Visual Studio (optional)
Contributors
Martin Svensson
License
This project is licensed under the MIT License.
