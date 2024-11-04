# Real-Time Admin Panel with SignalR

This project is a simple real-time admin panel example that uses **SignalR** and **Bogus**. It demonstrates real-time updates within an admin panel, built on **MVC architecture** for simplicity.

## Table of Contents
- [Real-Time Admin Panel with SignalR](#real-time-admin-panel-with-signalr)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Project Structure](#project-structure)
    - [Models](#models)
    - [Services](#services)
    - [SignalR Hub](#signalr-hub)
    - [Controllers](#controllers)
    - [Program.cs Setup](#programcs-setup)
    - [Testing with Swagger \& Postman](#testing-with-swagger--postman)
  - [Running the Project](#running-the-project)
  - [Technologies Used](#technologies-used)
  - [CI/CD Pipeline](#cicd-pipeline)

## Features
- Real-time data updates via **SignalR**
- Mock data generation with **Bogus**
- **Singleton** pattern for the statistics service
- In-memory data storage (no database required)
- API endpoint testing with **Swagger** and **Postman**

## Project Structure
The project follows the **Model-View-Controller (MVC)** architecture:

### Models
- **Statistic Model**: Stores admin data such as:
  - Total users
  - Active users
  - New users today
  - Total sales
  - Errors reported

### Services
- **Statistics Service**: Manages statistics without a database. Uses a private `_currentStatistics` field for in-memory data persistence.
  - **generateRandomStatistics()**: Creates fake statistics using **Bogus**.
  - **updateStatistics()**: Updates statistics based on client data and broadcasts changes to all connected users via **SignalR**.

### SignalR Hub
- **StatisticsHub**: Facilitates real-time data sharing with clients.
  - Sends current statistics upon user connection.
  - Broadcasts updates to all connected clients in real-time.

### Controllers
- **StatisticsController**: Provides API endpoints for managing statistics:
  - `GetCurrentStatistics()`: Retrieves current statistics.
  - `UpdateStatistics()`: Updates statistics with client data.
  - `ResetStatistics()`: Resets statistics with fresh random values, broadcasting the update in real-time.

### Program.cs Setup
- Configures **Singleton** for `StatisticsService`.
- Initializes **SignalR** and maps endpoints for controllers and the SignalR hub.

### Testing with Swagger & Postman
- **Swagger**: Used for testing RESTful endpoints (not for WebSocket connections).
- **Postman**: Supports WebSocket testing. Uses a specific ASCII character sequence (`0x1e`) sent in **Base64** format (`eyJwcm90b2NvbCI6Impzb24iLCAidmVyc2lvbiI6MX0e`) to establish a WebSocket handshake.

## Running the Project
1. Clone the repository.
2. Install dependencies.
3. Start the project using your IDE or .NET CLI.
4. Access the admin panel and API endpoints via Swagger.
5. Test real-time updates in **Postman** using WebSocket connections.

**Note**: All data is stored in memory for simplicity. Real-time updates are facilitated by **SignalR** through `HubContext` in `StatisticsService`.

## Technologies Used
- **C#**
- **ASP.NET Web API** (MVC pattern)
- **SignalR** (real-time communication)
- **Bogus** (data mocking)
- **Swagger** (API testing)
- **Postman** (WebSocket testing)

## CI/CD Pipeline
This project uses **GitHub Actions** for CI/CD to automate build, test, and deployment steps.

- **Docker Compose Files**:
  - `docker-compose-prod`: Production deployment.
  - `docker-compose-test`: Testing environments.
  - `docker-file-prod`: Production build.
  - `docker-file-test`: Test build.

- **Pipeline Steps**:
  1. **Build**: Runs on local setup with `docker-compose-prod`, `docker-compose-test`, and `docker-compose-api`.
  2. **Test**: Utilizes `docker-compose-test` for functionality validation.
  3. **Deploy**: Deploys via `docker-compose-app` after successful tests.

This CI/CD setup ensures streamlined building, testing, and deployment, enhancing development efficiency.