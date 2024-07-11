# DemoQA UI & API Automation Test Project

## Overview
This project is an automation test suite for the DemoQA UI & API, developed using .NET 8 with C# as the primary programming language, and NUnit 3 for testing.

## Project Structure
The solution comprises three projects:

1. **Core**: Contains utilities for API interaction, configuration file management, status code verification, schema validation, and data storage for teardown processes.
2. **DemoQA.Service**: Contains request and response models, as well as `UserServices` for sending API requests.
3. **DemoQA.APITesting**: Contains the test cases for `UserService` and `BookService`, test data, and configuration settings (`appsettings.json`).
3. **DemoQA.UITesting**: Contains the test cases for UI, test data, and configuration settings (`appsettings.json`).

## Development Tools
The project is set up using Visual Studio Code.

## Configuration File
The primary configuration file for this project is `appsettings.json`, which contains the application URL

## Running Tests
Before each test case, the token will be generated.

### Creating `account.json`

Create a `account.json` file in the `data` of your project to store your credentials. The format of the file should be as follows:

```account.json
{
   "Username": [Your Username],
   "Password": [Your Password],
   "Id": [Your Account Id]
}
```

### Using Visual Studio 2019
- Utilize the Test Explorer to select and run tests.

### Using Command Line
1. Restore all dependency packages:
   ```sh
   dotnet restore
   ```
2. Build the project:
   ```sh
   dotnet build
   ```
3. Run all tests:
   ```sh
   dotnet test
   ```
4. Run specific tests based on category:
   ```sh
   dotnet test --filter Category=GetUser
   ```
   (Replace `GetUser` with the desired test category.)
