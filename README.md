
# Real-Time Admin Panel with SignalR

This project is a simple real-time admin panel example that uses **SignalR** and **Bogus**. The purpose of this project is to showcase real-time updates in an admin panel environment using **MVC architecture** for simplicity.

## Features
- Real-time data updates using **SignalR**
- Mocking data with **Bogus**
- **Singleton** pattern for the statistics service
- No database interaction (in-memory data storage)
- Endpoints tested with **Swagger** and **Postman**
  
## Project Structure
The project uses the **Model-View-Controller (MVC)** architecture:

### Models
- **Statistic Model**: Holds admin data such as:
  - Total users
  - Active users
  - New users today
  - Total sales
  - Errors reported
  
### Services
- **Statistics Service**: Manages statistics without a database. It uses a private field (`_currentStatistics`) to store current statistics, which persist across sessions due to the **singleton** pattern.
  - **generateRandomStatistics()**: Generates fake statistics using **Bogus**.
  - **updateStatistics()**: Updates the statistics when data is received from the user and sends changes with **SignalR** to all connected users.
  
### SignalR Hub
- **StatisticsHub**: Handles real-time communication with clients.
  - When a user connects, the current statistics are returned.
  - When statistics are updated, all connected clients receive the updated statistics in real-time.

### Controllers
- **StatisticsController**: Provides endpoints for testing and interacting with statistics:
  - `GetCurrentStatistics()`: Returns current statistics.
  - `UpdateStatistics()`: Updates statistics with data sent from the client.
  - `ResetStatistics()`: Resets statistics by generating new random statistics and updating clients in real-time.

### Program.cs Setup
- **Singleton** for `StatisticsService`
- **SignalR** configuration and setup
- Mapped endpoints for controllers and the SignalR hub

### Testing with Swagger & Postman
- **Swagger** is used for endpoint testing but does not support WebSocket connections.
- **Postman** is used for WebSocket testing. To establish a handshake, a specific ASCII character sequence (`0x1e`) is used. This sequence is sent as binary data in **Base64** format (eyJwcm90b2NvbCI6Impzb24iLCAidmVyc2lvbiI6MX0e).

## Running the Project
1. Clone the repository
2. Install required dependencies
3. Run the project using your IDE or .NET CLI
4. Access the admin panel and API via Swagger
5. Test real-time updates via **Postman** using WebSocket connections

### Notes:
- The project does not use a database for simplicity. All data is stored in-memory.
- Real-time updates are enabled through **SignalR** and **HubContext** within the `StatisticsService`.

## Technologies Used
- **C#**
- **ASP.NET WebApi** (with MVC structure)
- **SignalR** (for real-time data)
- **Bogus** (for mocking data)
- **Swagger** (for API testing)
- **Postman** (for WebSocket testing)

## CI/CD Pipeline

This project uses a CI/CD pipeline configured with **GitHub Actions** to automate the build, test, and deployment processes. Here’s a brief overview of the key components:

- **Docker Compose Files**:
  - `docker-compose-prod`: Used for production deployment.
  - `docker-compose-test`: Used for testing environments.
  - `docker-file-prod`: Dockerfile for production builds.
  - `docker-file-test`: Dockerfile for test builds.

- **CI/CD Steps**:
  1. **Build**: The service is built using `docker-compose-prod`, `docker-compose-test`, and `docker-compose-api`. It runs on your local machine, but you can adjust the configuration for your own setup.
  2. **Test**: In the test stage, `docker-compose-test` is utilized to run tests. This ensures that all functionality is validated before deployment.
  3. **Deploy**: After successful tests, the service is deployed using `docker-compose-app`, making it available for use.

With this setup, you can ensure that your application is built, tested, and deployed efficiently, enhancing the overall development workflow.
