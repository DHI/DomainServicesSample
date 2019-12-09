# DHI Domain Services Sample ![DS logo](http://www.mikepoweredbydhi.com/upload/nuget-icons/domain-services-icon32x32.png)
A sample Domain Services Web API project extended with a custom controller using the same componentized, loosely-coupled, dependency-inverted architecture as the Domain Services themselves.

## Introduction

The purpose of this repository is to complement the Domain Services project templates and serve as a starter kit for new Domain Services based Web API solutions. It is meant to illustrate a solution structure where you use the Domain Services core - the `DHI.Services` NuGet package - as a framework for creating your own custom services and controllers. 

The solution contains an ASP.NET Core project based on the Domain Services Web API project template that comes with out-of-the-box functionality for user account management, authentication, logging etc. from the `DHI.Services.WebApi` NuGet package, but it is extended with a custom controller `MyEntityController` with CRUD functionality for a ficticious entity called `MyEntity`. In a real application the entity could be for example a Product, Order, Campaign or similar entity.

## ChemRegulator
The ChemRegulator project contains the definition of the the object model and service types, for example `MyEntityService`. These are all pure .NET types - so called POCOs (Plain Old CLR objects).

This approach has several advantages, for example that you can easily unit test `MyEntityService` and other types in the POCO project. Furthermore, the Web API itself - the controller - can be implemented as a very lean layer, essentially as a kind of façade exposing the pure .NET services over HTTP.

## ChemRegulator.Test

This projects contains the unit tests for the ChemRegulator types. The project uses the xUnit.NET test framework.

## ChemRegulator.WebApi

This is the original ASP.NET Core 2.2 project from the Domain Services Web API project template. However, it is extended with the custom `MyEntityController` class.

The MyEntityController contains a number of very short public methods (actions) that essentially redirect the job to the underlying `MyEntityService`.

The Domain Services artifacts, for example the `AuthenticationService`, are configured using the Domain Services' own Connections API (or by direct modifications in the `connections.json` file). However, the custom MyEntityService is configured in code using the built-in Dependency Injection (DI) capabilities of ASP.NET Core. This is done in the `ConfigureServices` method of the `Startup.cs` file: 

```c#
services.AddSingleton<IMyEntityRepository, FakeMyEntityRepository>();
```
The MyEntityService is configured to use a fake in-memory repository. In a real project, this should of course be replaced with for example a PostgreSQL repository or similar.

## ChemRegulator.WebApi.Test

This projects contains the integration tests for the ChemRegulator Web API. The project uses the xUnit.NET test framework and the `Microsoft.AspNetCore.Mvc.Testing` package.

## Running

The solution is configured to use the PostgreSQL provider (the `DHI.Services.PostgreSQL` package) for storage of user accounts and other Domain Services entities.

To run this project, you need to install PostgreSQL locally, create a database and configure the connections in `connections.json` with a valid PostgreSQL connection string.
