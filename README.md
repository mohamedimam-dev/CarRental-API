# CarRental API

## Project Overview

CarRental API is a RESTful Web API developed using ASP.NET Core 8 for managing a car rental system.

The project provides APIs for managing users, customers, vehicles, rental bookings, rental transactions, maintenance operations, and related entities.

The project was built with a focus on clean separation of responsibilities, secure authentication and authorization, asynchronous programming, validation, centralized service results, logging, and auditing.

The application uses Entity Framework Core with SQL Server and follows a service-based architecture where controllers are responsible for handling HTTP requests and responses while business operations are implemented in the service layer.

## Key Features

### User & Access Management

- User management.
- Role management.
- JWT-based authentication.
- Role-based authorization.
- Ownership-based authorization for protected resources.
- Secure password handling using BCrypt.
- User activation and deactivation.
- Password change functionality.
- Access and refresh token authentication.
- Refresh token rotation and revocation.

### Customer Management

- Customer creation and management.
- Customer-related rental operations.

### Vehicle Management

- Vehicle management.
- Vehicle categories.
- Fuel type management.
- Vehicle availability management.
- Vehicle creation, update, and deletion.

### Rental Management

- Rental booking management.
- Booking status management.
- Rental transactions.
- Vehicle return management.

### Vehicle Maintenance

- Maintenance record management.
- Maintenance status management.
- Maintenance completion tracking.

### Security, Logging & Auditing

- Security logging for authentication and security-related events.
- Audit logging for important create, update, delete, deactivate, and credential-related operations.
- Tracking the authenticated user responsible for audited operations.
- Recording client IP addresses for audited operations.

### API & Data Access

- RESTful API endpoints.
- DTO-based request and response models.
- Service layer for business operations.
- Service interfaces for defining service contracts.
- Entity Framework Core with SQL Server.
- Database First development approach.
- Asynchronous database operations using `async/await`.
- Structured `ServiceResult` responses for service operations.
- Input validation and appropriate HTTP status codes.
- Swagger/OpenAPI API documentation.

## Business Workflow

The system manages the vehicle rental lifecycle from booking and rental transactions through vehicle returns and maintenance.

### 1. Rental Booking

The process starts when a customer requests to rent a vehicle.

- A rental booking is created for the selected vehicle.
- The system validates the booking conditions and vehicle availability.
- The rental period and daily rental rate are used to determine the rental cost.
- The vehicle is marked as unavailable while it is assigned to an active rental.

```text
Customer
   ↓
Rental Booking
   ↓
Availability & Validation
   ↓
Rental Transaction
   ↓
Vehicle Unavailable
```

### 2. Rental Transaction

A rental transaction is created as part of the rental process.

The rental amount is calculated based on:

- The rental duration.
- The vehicle's daily rental rate.

The transaction is created automatically according to the rental operation and applicable rental period.

### 3. Vehicle Return

When the vehicle is returned:

- The return operation is recorded.
- The actual rental period is used to calculate the applicable rental amount.
- The resulting transaction information is recorded automatically.
- Any applicable amount due is determined as part of the return process.
- The vehicle becomes available again when the return process is completed successfully.

```text
Vehicle Return
      ↓
Rental Period Calculation
      ↓
Transaction Calculation
      ↓
Settlement
      ↓
Vehicle Available
```

### 4. Vehicle Maintenance

When a vehicle requires maintenance:

- A maintenance record is created.
- The vehicle becomes unavailable for rental.
- The maintenance status tracks the maintenance lifecycle.

```text
Maintenance Required
        ↓
Maintenance Record
        ↓
Vehicle Unavailable
        ↓
Maintenance In Progress
```

### 5. Maintenance Completion

When maintenance is completed:

- A maintenance completion record is created.
- The maintenance lifecycle is completed.
- The vehicle becomes available for rental again.

```text
Maintenance Completed
        ↓
Maintenance Completion
        ↓
Vehicle Available
```

### Overall Vehicle Lifecycle

```text
                    ┌─────────────────┐
                    │    Available    │
                    └────────┬────────┘
                             │
                       Rental Booking
                             │
                             ▼
                    ┌─────────────────┐
                    │   Unavailable   │
                    │    (Rental)     │
                    └────────┬────────┘
                             │
                       Vehicle Return
                             │
                             ▼
                    ┌─────────────────┐
                    │    Available    │
                    └────────┬────────┘
                             │
                    Maintenance Required
                             │
                             ▼
                    ┌─────────────────┐
                    │   Unavailable   │
                    │  (Maintenance)  │
                    └────────┬────────┘
                             │
                   Maintenance Completed
                             │
                             ▼
                    ┌─────────────────┐
                    │    Available    │
                    └─────────────────┘
```

## Architecture

The project follows a service-based architecture with a clear separation of responsibilities:

```text
HTTP Request
     ↓
Controllers
     ↓
Service Interfaces
     ↓
Services
     ↓
Entity Framework Core / DbContext
     ↓
SQL Server
```

### Controllers

Controllers are responsible for:

- Receiving HTTP requests.
- Basic request validation.
- Authorization checks.
- Calling the appropriate service.
- Mapping service results to appropriate HTTP responses.

Controllers do not contain the application's core business logic.

### DTOs

Data Transfer Objects (DTOs) are used to define explicit API request and response contracts.

They help:

- Prevent exposing database entities directly through the API.
- Separate API models from database entities.
- Control the data received from and returned to clients.
- Provide request-specific validation rules.

### Service Interfaces

Service interfaces define the contracts for the application's business services.

They provide:

- Clear separation between controllers and service implementations.
- Dependency Injection support.
- Better maintainability and testability.
- Consistent service contracts.

### Services

The service layer contains the application's business operations and database interaction logic.

Services are responsible for:

- Business validations.
- Entity operations.
- Database queries and updates.
- Applying application business rules.
- Returning structured `ServiceResult` objects to controllers.

### Entities

Entity classes represent the database tables and relationships used by Entity Framework Core.

### Enums

Enums are used to represent predefined application values such as:

- Audit actions.
- Audit entities.
- Security log types.
- Other fixed application states and operations.

This improves consistency and avoids using hard-coded string values throughout the application.

### Data Access

Entity Framework Core is used directly through the application's `DbContext`.

The project uses the **Database First** approach with SQL Server.

No Repository Pattern is used. Services interact directly with the Entity Framework Core `DbContext`, keeping the data-access layer simple and avoiding unnecessary abstraction.

## Authentication & Authorization

The API implements secure authentication and authorization using **JWT Bearer Authentication**, **BCrypt password hashing**, and **Refresh Token** management.

### Authentication Flow

The authentication flow uses short-lived access tokens together with refresh tokens to maintain authenticated sessions securely.

### Login

The login endpoint:

1. Validates the user's credentials.
2. Verifies the password using BCrypt.
3. Checks whether the user account is active.
4. Generates an **Access Token**.
5. Generates a **Refresh Token**.
6. Stores the refresh token securely for subsequent token operations.
7. Returns both tokens to the client.

```text
Username + Password
        ↓
      Login
        ↓
 Credential Verification
        ↓
Access Token + Refresh Token
```

### Refresh Token

The API provides a refresh token endpoint that allows clients to obtain a new access token without requiring the user to log in again.

The refresh flow:

1. Receives the refresh token and user identity.
2. Validates the refresh token.
3. Checks its expiration and revocation status.
4. Generates a new access token.
5. Generates a new refresh token.
6. Replaces the previous refresh token.
7. Returns the new access token and refresh token.

```text
Refresh Token
      ↓
   Validation
      ↓
New Access Token
      +
New Refresh Token
```

This implements **refresh token rotation**, reducing the risk associated with the reuse of long-lived refresh tokens.

### Logout

The logout endpoint revokes the user's refresh token.

Once revoked, the refresh token can no longer be used to obtain new access tokens.

```text
Logout
   ↓
Refresh Token Revocation
   ↓
Refresh Token Cannot Be Reused
```

### Authorization

The API implements multiple authorization mechanisms:

- **Role-based authorization** for administrative operations.
- **Ownership-based authorization** to ensure users can only access or modify resources they are authorized to manage.
- Centralized authorization handlers for reusable authorization rules.
- Administrator override for operations requiring elevated privileges.

### Secure User Context

Sensitive user identifiers required for server-side operations are obtained from the authenticated user's JWT claims rather than being trusted from client-provided request data.

For example, when creating a vehicle, the `CreatedByUserId` is derived from the authenticated user's JWT identity instead of being accepted from the request DTO.

## Logging & Auditing

The API includes dedicated logging and auditing mechanisms to improve security, traceability, and operational visibility.

### Security Logging

Security-related events are recorded through a dedicated security logging service.

Examples include:

- Successful authentication attempts.
- Failed authentication attempts.
- Other security-related events.

Security logs help provide visibility into authentication and security activities within the system.

### Audit Logging

Audit logging is used to track important operations performed on application entities.

Audited operations include:

- Create operations.
- Update operations.
- Delete operations.
- Deactivation operations.
- Credential-related operations.

Each audit record can include:

- The authenticated user who performed the operation.
- The performed action.
- The affected entity.
- The affected entity identifier.
- The client's IP address.
- The timestamp of the operation.

### Audit Structure

Audit actions and audited entities are represented using dedicated enums to provide consistent and controlled values throughout the application.

```text
Authenticated User
        ↓
   API Operation
        ↓
   Audit Logging
        ↓
User + Action + Entity + Entity ID + IP Address + Timestamp
```

The logging and auditing components are implemented through dedicated interfaces and services and are integrated into the relevant API operations through Dependency Injection.

## Database & Data Access

The project uses **Microsoft SQL Server** as the relational database and **Entity Framework Core 8** for data access.

### Database Design

The database contains the main entities required to manage the car rental workflow, including:

- Users and Roles.
- Customers.
- Vehicles.
- Vehicle Categories.
- Fuel Types.
- Rental Bookings.
- Booking Statuses.
- Rental Transactions.
- Vehicle Returns.
- Maintenance Records.
- Maintenance Statuses.
- Maintenance Completions.
- Security Logs.
- Audit Logs.

The database includes:

- Primary and foreign key relationships.
- Unique constraints.
- Required fields.
- Validation constraints.
- Referential integrity.
- Appropriate relationships between entities.

### Entity Framework Core

The API uses Entity Framework Core with the **Database First** approach.

The application's `DbContext` provides direct access to the database while the service layer contains the database operations and business rules.

The project does not use the Repository Pattern. Services interact directly with the EF Core `DbContext` to keep the data-access layer simple and avoid unnecessary abstraction.

### Querying and Related Data

The project uses **LINQ** extensively for querying, filtering, and projecting data through Entity Framework Core.

The following LINQ techniques are used where appropriate:

- `Where` for filtering records.
- `Select` for projecting only the required fields.
- `Include` and `ThenInclude` for loading related entities when required by a specific query.
- Other LINQ operators such as `Any`, `FirstOrDefault`, and ordering operations where appropriate.

Queries are designed to retrieve only the data required by each operation whenever practical, helping avoid unnecessary data loading and improving query efficiency.

### Asynchronous Data Access

Database operations are implemented using asynchronous EF Core methods such as:

- `ToListAsync()`
- `FirstOrDefaultAsync()`
- `FindAsync()`
- `AnyAsync()`
- `SaveChangesAsync()`

This allows the API to handle database operations without blocking request threads.

### Database Setup

A complete SQL Server database script is included in the repository:

```text
Database/
└── CarRentalDB.sql
```

The script can be used to recreate the CarRental database locally, including its schema, tables, relationships, constraints, and required database objects.

## API Response Handling & Error Management

The API follows a consistent approach for handling service results, validation, and HTTP responses.

### Service Results

The service layer uses a generic `ServiceResult<T>` structure to communicate operation outcomes to controllers.

Service operations can return statuses such as:

- `Success`
- `NotFound`
- `BadRequest`
- `Conflict`

This keeps business operation results separate from HTTP-specific response handling.

### HTTP Status Codes

Controllers translate service results into appropriate HTTP responses, including:

- `200 OK` for successful operations that return data.
- `201 Created` where applicable for successful resource creation.
- `204 No Content` for successful operations without a response body.
- `400 Bad Request` for invalid requests or business validation failures.
- `401 Unauthorized` for unauthenticated requests.
- `403 Forbidden` for authenticated users without sufficient permissions.
- `404 Not Found` when the requested resource does not exist.
- `409 Conflict` when an operation conflicts with the current state of the resource.
- `500 Internal Server Error` for unexpected server-side failures.

### Validation

The API performs request validation before executing service operations.

Validation is handled through:

- Data annotation attributes on DTOs.
- Model state validation in controllers.
- Additional business validation within the service layer.

This approach keeps basic HTTP/request validation in the API layer while keeping business rules within the service layer.

## Project Structure

The project is organized into clear folders based on responsibility:

```text
CarRental.API/
│
├── Authorization/
│   └── Custom authorization requirements and handlers
│
├── Common/
│   └── Shared application components and service result handling
│
├── Controllers/
│   └── API controllers and HTTP endpoint handling
│
├── DTOs/
│   └── Request and response data transfer objects
│
├── Entities/
│   └── Entity Framework Core database entities
│
├── Enums/
│   └── Application enums for controlled values and operations
│
├── Services/
│   ├── Interfaces/
│   │   └── Service contracts
│   └── Service implementations
│
├── Database/
│   └── SQL Server database creation script
│
├── Program.cs
│   └── Application configuration and dependency injection
│
└── appsettings.json
    └── Application configuration and connection settings
```

This structure keeps API endpoints, business logic, data models, authorization, and shared components separated and easier to maintain.

## API Documentation & Testing

The API is documented and tested using **Swagger / OpenAPI**.

Swagger provides an interactive interface for exploring and testing the available endpoints.

Swagger can be used to:

- Explore available API endpoints.
- Review request and response models.
- View required parameters and validation rules.
- Authenticate using a JWT Bearer token.
- Execute API requests directly from the browser.
- Review HTTP status codes and API responses.
- Test authentication, authorization, validation, CRUD operations, and different success and failure scenarios.

## Getting Started

### Prerequisites

Before running the project, make sure the following are installed:

- .NET 8 SDK
- Microsoft SQL Server
- Visual Studio 2022 or later

### Database Setup

1. Clone the repository:

```bash
git clone https://github.com/mohamedimam-dev/CarRental-API.git
```

2. Open the project in Visual Studio.

3. Create the database using the provided SQL script:

```text
Database/CarRentalDB.sql
```

4. Execute the script in SQL Server to create the `CarRentalDB` database and its required database objects.

### Configuration

Update the connection string in `appsettings.json` to match your local SQL Server configuration.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CarRentalDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Configure the JWT settings required by the application:

```json
{
  "JWT": {
    "SecretKey": "YOUR_SECRET_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "ExpirationInMinutes": 15
  }
}
```

> **Note:** Do not commit real credentials, secret keys, or production connection strings to source control.

### Run the Application

Build and run the project from Visual Studio.

Once the application starts, open the Swagger UI to explore and test the API endpoints.