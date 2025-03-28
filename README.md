# Human Capital Management App

This repository contains the code for a Human Capital Management (HCM) application. The project is designed to manage employee information.

## Functionalities
This application supports creating user profiles and managing roles. The roles are:

- **HR_Admin**: Can edit other users' profiles and roles.
- **Manager**: Can edit profiles of users that are in their department.
- **Employee**: Can view their own profile.

Every new user of the system receives the role "Employee".

## Usage
To run the application, follow these steps:

1. Build the project:
    ```bash
    dotnet build
    ```
2. Run the project:
    ```bash
    dotnet run
    ```
## Architecture 
The architecture of this HCM app is based on the Model-View-Controller (MVC) pattern. 

### Solution decomposition
The project consists of several class libraries that encapsulate different aspects of the application:

- Core API (__HR_system__): Contains controllers and views.
- Data Library (__HR_system.Data__): Manages data access and includes the repository pattern for data operations.
- Service Library (__HR_system.BLogic__): Implements various services used across the application, such as data manipulation and business rules.
- Constants Library (__HR_system.Constants__): It manages the Constants in the solution.
- Test Library (__HR_system.Test__): It keeps unit and infrastructure tests.

### In-Memory Database
For simplicity and ease of development, the application uses an in-memory database.
    
