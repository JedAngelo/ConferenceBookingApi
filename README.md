# ConferenceBookingAPI: RESTful Backend for Conference Room Management

## Overview

The `ConferenceBookingAPI` is a robust and secure RESTful API built with ASP.NET Core 8.0. It serves as the backend for a conference room booking system, managing conference rooms, bookings, holidays, and user authentication with role-based access control. The API is designed to be scalable and maintainable, following best practices for modern web development.

## Features

*   **Conference Room Management:**
    *   CRUD operations for `Conference` entities (conference rooms).
    *   Associate administrative users with specific conference rooms for management.
*   **Booking Management:**
    *   Create, retrieve, update, and delete conference `Booking` records.
    *   Handles both single and recurring bookings (daily, weekly, monthly).
    *   Checks for booking conflicts and manages booking statuses (e.g., pending, approved, rejected, extended).
    *   Allows extending existing bookings.
*   **Holiday Management:**
    *   CRUD operations for `Holiday` entities, used to mark non-bookable dates.
*   **User Authentication & Authorization:**
    *   Implemented using ASP.NET Core Identity for secure user management.
    *   Supports multiple roles: `SuperAdmin`, `AdminRole`, and `UserRole`.
        *   `SuperAdmin`: Has full control, including registering other SuperAdmins and Admins.
        *   `AdminRole`: Manages conferences, bookings, and holidays for assigned conferences.
        *   `UserRole`: Can create and view their own bookings.
    *   JWT (JSON Web Tokens) for secure API communication.
*   **API Documentation:** Integrated Swagger UI for interactive exploration and testing of all API endpoints.
*   **Cross-Origin Resource Sharing (CORS):** Configured to allow requests from various client applications.

## Technologies Used

*   **Backend Framework:** ASP.NET Core 8.0
*   **Database:** SQL Server
*   **ORM:** Microsoft.EntityFrameworkCore 8.0.10
*   **Authentication/Authorization:** Microsoft.AspNetCore.Identity, Microsoft.AspNetCore.Authentication.JwtBearer
*   **API Documentation:** Swashbuckle.AspNetCore 6.6.2
*   **Optional (HTTPS):** LettuceEncrypt for Let's Encrypt certificates (if configured for HTTPS).

## Project Structure

*   `ConferenceBookingAPI/`: The main ASP.NET Core Web API project.
    *   `Controllers/`: Defines API endpoints for `Booking`, `Conference`, `Holiday`, and `UserAuth`.
    *   `Migrations/`: Entity Framework Core database migration files.
    *   `Model/`: Database entities (`Booking`, `Conference`, `Holiday`, `ConferenceBookingContext`).
    *   `Model/Dto/`: Data Transfer Objects (DTOs) for various entities, ensuring clear data contract with clients. Includes DTOs for user authentication (`UserAuthDto`).
    *   `Services/`: Implements the core business logic for `BookingService`, `ConferenceService`, and `HolidayService`.
    *   `UserAuth/`: Contains custom ASP.NET Core Identity implementations, including `ApplicationUser`, `ApplicationDbContext`, and `UserAuthenticationService`.
    *   `appsettings.json`, `appsettings.Development.json`: Configuration files for database connection strings, JWT settings, and logging.
    *   `Program.cs`: Configures services (DB contexts, Identity, JWT authentication, custom services) and sets up the HTTP request pipeline.

**Important Note regarding `ApplicationDbContext`:**
As mentioned in the original `README.md`, `ApplicationDbContext` manages the `ApplicationUser` and `Conference` entities directly due to a foreign key relationship (`ConferenceId` in `ApplicationUser`). For database migrations (`Add-Migration`, `Update-Database`), ensure you target `ApplicationDbContext`. `ConferenceBookingContext` is used for other core booking-related entities.

## Setup Instructions

To get the `ConferenceBookingAPI` running, follow these steps:

### Prerequisites

1.  **.NET 8.0 SDK:** Download and install the .NET 8.0 SDK from the official Microsoft website.
2.  **SQL Server:** An instance of SQL Server (e.g., SQL Server Express, LocalDB, or a full SQL Server installation) is required.
3.  **SQL Server Management Studio (SSMS)** (Optional, but recommended for database inspection).

### Installation

1.  **Clone the Repository:**
    ```bash
    git clone https://github.com/jedangelo/conferencebookingapi.git
    cd jedangelo-conferencebookingapi
    ```

2.  **Open in Visual Studio:**
    Open the `ConferenceBooking.sln` file in Visual Studio (2022 recommended).

3.  **Restore NuGet Packages:**
    Visual Studio should automatically prompt you to restore missing NuGet packages. If not, right-click on the solution in Solution Explorer and select "Restore NuGet Packages".

4.  **Configure Database Connection String:**
    *   Open `ConferenceBookingAPI/appsettings.json`.
    *   Locate the `"ConnectionStrings"` section.
    *   Update the `"DefaultCon"` value to point to your SQL Server instance.
        ```json
        "ConnectionStrings": {
          "DefaultCon": "Data Source=YOUR_SERVER_NAME\\YOUR_INSTANCE_NAME;Initial Catalog=ConferenceBookingDB;User Id=sa;Password=Stone123;TrustServerCertificate=True"
        }
        ```
        Replace `YOUR_SERVER_NAME` and `YOUR_INSTANCE_NAME` with your actual SQL Server details. Adjust `User Id` and `Password` if you're not using SQL Server authentication or if your `sa` password is different. For Windows Authentication, you might use `Integrated Security=True;`.

5.  **Apply Database Migrations:**
    *   Open the Package Manager Console in Visual Studio (`Tools > NuGet Package Manager > Package Manager Console`).
    *   Ensure the "Default project" is set to `ConferenceBookingAPI`.
    *   Run the following command to apply the database migrations and create the database schema:
        ```powershell
        Update-Database
        ```
        This command will create the `ConferenceBookingDB` database (if it doesn't exist) and apply all defined migrations.

6.  **Configure JWT Secret Key:**
    *   In `ConferenceBookingAPI/appsettings.json`, locate the `"JWT"` section.
    *   The `SecretKey` is currently hardcoded. For production environments, it is crucial to manage this securely (e.g., via environment variables).
        ```json
        "JWT": {
          "ValidAudience": "User",
          "ValidIssuer": "https://localhost:2401/",
          "Configuration": [
            "JWT:ValidIssuer"
          ],
          "SecretKey": "TheTreesMightLongForPeaceButTheWindWillNeverCeaseTheQuickBrownFoxJumpsOverTheLazyDog"
        }
        ```
        Ensure the `ValidIssuer` value matches the `applicationUrl` (or `sslPort` if using HTTPS) configured in `Properties/launchSettings.json` or your deployment environment.

### Running the API

1.  **From Visual Studio:**
    *   Select `ConferenceBookingAPI` as the startup project.
    *   Press `F5` or click `Debug > Start Debugging`. This will launch the API and open the Swagger UI in your browser.
2.  **From Command Line:**
    *   Navigate to the `ConferenceBookingAPI` directory in your terminal.
    *   Run: `dotnet run`
    *   The API will typically run on `http://*:5053` (HTTP) and `https://*:7026` (HTTPS) by default (check `Properties/launchSettings.json` for exact ports and host configurations).

### Initial User Setup

After the database is set up and the API is running, you will need to register an initial `SuperAdmin` user to gain full control of the system.

1.  **Register SuperAdmin (via Swagger UI):**
    *   Go to the Swagger UI in your browser (e.g., `https://localhost:7026/swagger`).
    *   Find the `UserAuth` controller.
    *   Expand the `POST /api/UserAuth/RegisterSuperAdmin` endpoint.
    *   Click "Try it out" and then "Execute". Provide a username, password, and email.
    *   This will create the first `SuperAdmin` account. Subsequent `AdminRole` users can be registered by a `SuperAdmin`.

## API Endpoints

Once the API is running, you can explore the available endpoints using the integrated Swagger UI:

*   **Swagger UI (Development):** `https://localhost:7026/swagger` (or your configured HTTPS/HTTP URL + `/swagger`)
