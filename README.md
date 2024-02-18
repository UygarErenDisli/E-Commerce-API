# E-CommerceAPI
### Getting Started
These instructions will get you a copy of the project up and 
running on your local machine for development and testing purposes.
### About This Project
This is a Back-end api for [E-CommerceClient](https://github.com/UygarErenDisli/ECommerceClient)
## Used Packages

- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - EF Core can serve as an object-relational mapper
- [PostgreSQL](https://www.postgresql.org/) - PostgreSQL is a powerful, open source object-relational database.
- [FluentValidation](https://docs.fluentvalidation.net/en/latest/) - FluentValidation is a . NET library for building strongly-typed validation rules
- [Serilog](https://serilog.net/) - Serilog is an easy-to-set-up logging library for .NET with a clear API.
- [MediatR](https://github.com/jbogard/MediatR) - MediatR helps in decoupling the components of an application
- [AzureStorageBlobs](https://github.com/Azure/azure-sdk-for-net/blob/Azure.Storage.Blobs_12.20.0-beta.1/sdk/storage/Azure.Storage.Blobs/README.md) - Azure storage blobls allows us to connect to Azure portal.
- [GoogleApisAuth](https://github.com/googleapis/google-api-dotnet-client) - The Google APIs Client Library is a runtime client for working with Google services.
- [JWTBearer](https://github.com/dotnet/aspnetcore) - ASP .NET Core middleware that enables an application to receive an OpenID Connect bearer token.
- [SignalR](https://github.com/aspnet/SignalR) - Real-time communication framework for ASP .NET Core.

## Used Architectures and Design Patterns

- [Onion Architecture ](https://www.linkedin.com/pulse/onion-architecture-arshad-shahoriar-r6ehc/) - 
Onion architecture organizes software into layers, with the core domain logic at the center, promoting modularity and testability while keeping concerns separated.
- [CQRS Pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs) - CQRS stands for Command and Query Responsibility Segregation, a pattern that separates read and update operations for a data store.
- [Mediator Pattern](https://serilog.net/) - 
MediatR simplifies communication between components in .NET by implementing the Mediator pattern. It reduces coupling, enhances maintainability, and promotes cleaner code by separating requests and their handlers.
- [Repository Design Pattern](https://www.linkedin.com/pulse/what-repository-pattern-alper-sara%C3%A7/) - 
The Repository Pattern abstracts data access, separating concerns, and simplifying CRUD operations, which leads to cleaner, more maintainable code.


## Requirements

- .NET Framework [here](https://dotnet.microsoft.com/en-us/download)
- Visual Studio [here](https://visualstudio.microsoft.com/downloads/) or IDE of your choice.
- Docker [here](https://docs.docker.com/desktop/).
- A Client [here](https://github.com/UygarErenDisli/ECommerceClient).
- (Optional)  Azure Portal Account for Storage Service  [here](https://azure.microsoft.com/en-us/get-started/azure-portal).
- (Optional) Google Cloud for google login [here](https://cloud.google.com/?hl=en).
- (Optional) DbBeaver for viewing database [here](https://dbeaver.io/download/).


## Installation
- Need a connection string
    - To get a connection string.
    - Start Docker at the background, open a terminal and copy and paste below command.
       - `docker run --name PostgreSQL -p 5432:5432 -e POSTGRES_PASSWORD=123456 -d postgres`  
            -  Above command will download PostgreSQL latest version to your docker app and create a container.
            -  If you run the command succussfully you can start the container by `docker start PostgreSQL` command.
                - You can see all containers by docker ps -a command.  
            -  Now you can use the line below as a connection string.
            -  `UserID=postgres;Password=123456;Host=localhost;Port=5432;Database=ECommerceAPIDb;`
            -  And don't forget to put it in `appsettings.json`
- After getting connection string clone the repository
- After putting connection string next thing, we need to do is a add a migration. To do that:
    - Make sure PostgreSQL container is running
    - Then you need to set `E-Commerce.API` as a start up project. By right clicking to project and click `Set as Startup Project`
    - Open Package Manager Console.
    - Select `E-Commerce.Persistence` as default project, in Default project section.
    - Then type `add-migration SeedData` and press enter
    - After migration successfully created run `update-database` command
    - If you see `Done.` messsage in console everything is done.
        - If you want to view tables and diagrams you can use DbBeaver app [here](https://dbeaver.com/docs/dbeaver/Database-driver-PostgreSQL/)  
- Now you can choose how you want to storage product images
    - You can use Local Storage
        - First thing you need to do is, in the `Program.cs` you need uncomment this `builder.Services.AddStorage<LocalStorage>();` line and comment or delete this `builder.Services.AddStorage<AzureStorage>();` line.
        - Last thing you need to do is create a folder called `wwwroot` in `E-Commerce.API` layer.
	- You can use Azure Portal
	    - You need to setup a Storage acount. [documentation](https://learn.microsoft.com/en-us/azure/storage/common/storage-account-create?tabs=azure-portal) 
	    - If you setup correctly you need to get connection string from azure storage acount. You can get it in the access key section in azure portal. When you get connection string you can put it in `appsettings.json`
	    - Then you need to copy your Base Storage Url
	        - Base Storage Url example  => `https://[NameOfYourBlob].blob.core.windows.net`
			- To find this you need to manually upload a image to your blob
			- When you upload your image you can click and copy your url from the panel that shows up.
			- Ful Storage Url example => `https://[NameOfYourBlob].blob.core.windows.net/[NameOFYourImageFile]`
			- Then you can copy your base storage url  => `https://[NameOfYourBlob].blob.core.windows.net`   ***Without ['/']***
			- Then you can paste it in to `appsettings.json` BaseStorageUrl Section 
		    - By default, azure storage service is selected you don't need to do anything else.
	- You can create your own Storage Service
	    - First you need to create an Interface for your service `IExampleStorageService`
		- Make sure your interface implements `IStorage` interface ***Important***
		- Then you need to do a create a class for your storage service `ExampleStorageService`
		- Make sure that your storage service inherits `Storage` class, and implements your Interface `IExampleStorageService` ***Important***
		- And implement your storage service and create all required methods
		- After implemented your storage service all you need to do is in `Program.cs` you need to comment or delete => `builder.Services.AddStorage<AzureStorage>();` and add your storage service just like other storage services `builder.Services.AddStorage<ExampleStorageService>();`
- Setting up google login. ***You don't need to this steps!***  
	- You need to have a Google cloud acount.You can find documentation [here](https://developers.google.com/identity/gsi/web/guides/overview?hl=en)
	- After setting up everything, all you need to do is get a google client id and put it in `appsettings.json` GoogleClintId section.
	- And that's it
- Now if you successfully done all steps above you need to setup [E-CommerceClint](https://github.com/UygarErenDisli/ECommerceClient)
    - CORS policy is already set for default angular hosts origins.
    - If you changed angular origins, you need to specify in `Program.cs`
- When running the project you must use http version not https. ***Important***
