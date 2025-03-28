# Human Capital Management App

This repository contains the code for a Human Capital Management (HCM) application. The project is designed to manage employee information.

## Functionalities
This application supports creating user profiles and managing roles. The roles are:

- **HR_Admin**: Can edit other users' profiles and roles.
- **Manager**: Can edit profiles of users that are in their department.
- **Employee**: Can view their own profile.

Every new user of the system receives the role "Employee".
## Installation Instructions
To set up the project locally, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/ViktorKapra/ukg_be_assignment_hcm_Viktor_Kapra.git
    ```
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
The architecture of this Human Capital Management app is based on the Model-View-Controller (MVC) pattern. This pattern separates the application into three main components:

### Class Libraries
The project consists of several class libraries that encapsulate different aspects of the application:

- Core API (__HR_system__): Contains the core business logic and domain models.
- Data Library(__HR_system.Data__): Manages data access and includes the repository pattern for data operations.
- Service Library (__HR_system.BLogic__): Implements various services used across the application, such as data manipulation and business rules.
- Constants Library (__HR_system.Constants__): It manages the Constants in the solution
- Test Library (__HR_system.Test__): It keeps unit and infrastructure tests

### In-Memory Database
For simplicity and ease of development, the application uses an in-memory database.
    
