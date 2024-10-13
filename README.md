
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
  - **updateStatistics()**: Updates the statistics when data is received from the user and sents via **SignalR** all connected users.
  
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
- **Postman** is used for WebSocket testing. To establish a handshake, a specific ASCII character sequence (`0x1e`) is used. This sequence is sent as binary data in **Base64** format.

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
- **ASP.NET WebApi**
- **SignalR**
- **Bogus** (for mocking data)
- **Swagger** (for API testing)
- **Postman** (for WebSocket testing)
