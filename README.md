# Companies Reviewing System
It's a web application platform that is responsible for reviewing and scoring the companies and the employees have the right to write their feedback for the companies they had worked for, analyzing these _**english written reviews**_ using a **Machine Learning** approach.

## To view your subscription credits go to [Azure Sponsorships Balance](https://www.microsoftazuresponsorships.com/Balance).

# To run this project on your machine

## Requirements

* SQL Server
* * Visual Studio 2019 and install on it
	* ASP.Net and Web Development
	* Azure Development
	* .Net Desktop Development
	* .Net Cross-platform Development
	* Desktop Development with c++ (optional)
	* Mobile  Development with .Net (optional)
* Git to push and pull through Github. (You can see how to link your Github account with visual studio through [this link](https://github.com/github/VisualStudio/blob/master/docs/getting-started/authenticating-to-github.md)).

## Project Structure
### `Controllers` Folder
Carries the controllers for each model.
### `Data` Folder
For database context class.
### `Dtos` Folder
Data transfer objects that are another forms of the models. For example if the user wants to see the user information like username and email only so we cannot return an object of type `user` because `user` carries additional information like password so we create another class called DTO which is a subset from `user`.
### `Migrations` Folder
Database migrations that converts codes to queries.
### `Models` Folder
Database tables and main models of the system.
### `Utilities` Folder
Some utility classes and functions that used among the system.
### `appsettings.json` File
Carries the settings of the application especially database connections.
### `MapProfiles.cs` File
Mapping is the way to convert a model to DTO.
### `Program.cs` File
The main function.
### `Startup.cs` File
Initialization of the services before starting the application.

## Running

1. Go to `appsettings.json` then `"ConnectionStrings"` and add your database connection in this JSON and give it a name like `YourDatabase`.
2. Go to `startup.cs` and navigate to `ConfigureServices` function then the line that contains `services.AddDbContext` change the parameter that inside `GetConnectionString` function to your database connection name like `YourDatabase`.
The line should be like 
```c#
services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(Configuration.GetConnectionString("YourDatabase")));
```
3. Then in visual studio go to tools -> NuGet Package Manager -> Package Manager Console
4. In the console write these 2 commands:
`add-migration db`
`update-database`
These commands will create the database on the given connection string.
5. Check that you installed all the dependencies and run the application by hitting `ctrl` + `F5`

## Deployment
We mainly deploy on azure with the following steps:
1. Create an online database server.
2. Create a database inside this server.
3. Link the database with the API through database connection.
4. Deploy the API.
### Database Deployment
1. Go to [azure portal](https://portal.azure.com/#home).
2. Open the menu at the left and choose `SQL Databases`.
3. Choose `Create`.
4. Subscription: `Azure for Students`.
5. Resource Group: create one and name it anything or choose anyone from the menu.
6. Database Name: write the database name that you want.
7. Server: Create New if not exists, to create a server:
	1. Enter server name and location `East US`.
	2. Authentication Method: `Use SQL authentication`.
	3. Enter server admin login and password.
	4. Press Ok.
8. For Compute + storage choose `Configure database` then choose Service tier: Standard then apply.
9. Then at the bottom choose `Review + Create`.
10. In the notifications wait the database to be created (refresh frequently). 
11. Once the database is created go to home then the menu at the left and choose `SQL Databases`.
12. You should see your database, click on it to open it.
13. At the right you will see `Server name`, write click on the server name to open the server configurations.
14. In the left menu find Security section and choose Firewall and virtual networks.
15. Add a new rule with the following properties to allow you to access the server from any computer:
	1. Rule Name: All
	2. Start IP: 0.0.0.0
	3. End IP: 255.255.255.255
16. Click save on the top.
17. Go back to home then the menu at the left and choose `SQL Databases` again and choose your database.
18. On the right you will see `Connection Strings`.
19. Press on Show database connection strings.
20. Copy the connection string and paste it in notepad.

### API and Database Linking
1. In the API go to `appsettings.json`.
2. In `"ConnectionStrings"` JSON object add a new object like `"YourDatabase": "your connection string goes here"`
3. And open `Startup.cs` in `// Database connection` section in function `services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("YourDatabase")));` add the connection name in the function like the previous code.
4. Save files.
5. Open the project in visual studio.
6. In tools bar choose `Tools` then `NuGet Package Manager` and choose `Package Manager Console`
7. In the console type `update-database` to create database columns.
8. Run the application and test it if it works.
             
### API Deployment
1. Go to [azure portal](https://portal.azure.com/#home).
2. Open the menu at the left and choose `SQL Databases`.
3. Choose `Create`.
4. Subscription: `Azure for Students`.
5. Resource Group: create one and name it anything or choose anyone from the menu.
6. Name: write the name you want.
7. Publish: code.
8. Runtime stack: .Net 5.
9. Operating System: Linux.
10. Region: East US
11. Sku and Size: Change Size and choose B1 
12. Then at the bottom choose `Review + Create`.
13. In the notifications wait the app service to be created (refresh frequently). 
14. Once the database is created go to home then the menu at the left and choose `App Services`.
15. You should see your app service here, click on it.
16. In the left go to API section and choose CORS and in the allowed origins add `*` then click save at the top (this to enable you to access the API from any application and IP).
17. Then in the left go to settings section and choose Configuration then choose General Settings tab and make Always on is true.
18. Then open the project on visual studio.
19. In the solution explorer right click on the project name `Interactive-Dashboard-API` and choose publish.
20. Choose target: Azure.
21. Specific target: Azure app service (Linux).
22. Login to your Microsoft account.
23. Choose the app service that you created and next.
24. In the next step choose `Skip this Step` and next.
25. Deployment type: Publish.
26. Then finish.
27. Then click publish.
28. After publishing navigate to `{your api link}/swagger` to see the API documentation.

## Our Team
- Ahmed Nasr ElDardery
- Belal Hamdy Ezzat
- Abdelrahman Ibrahim Ibrahim
- Ahmed Abdelaziz Hussnien

