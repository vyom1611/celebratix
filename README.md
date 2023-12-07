# CelebraTix

## Overview
CelebraTix is a .NET-based solution designed to streamline the management and promotion of events. It encompasses a wide range of functionalities including event creation, scheduling, and promotion. The project is structured into several key modules:

Acts: Management of acts including their details, descriptions, and status (active or removed).
Venues: Venue handling, including descriptions, locations, time zones.
Shows: Scheduling and management of shows, including cancellations.
Contents: Handling of content associated with acts and events, including image management.
Messaging: Facilitates communication and notifications related to shows and acts.
The solution is built with scalability and modularity in mind, allowing for easy integration and expansion.



**Note:** This project is currently under development. The description below outlines the intended final state of the application.

## Key Features (Planned)
- **Act Management**: Manage performance acts, including their creation, updates, and removal.
- **Venue Management**: Oversee venue details, including descriptions, locations, and time zones.
- **Show Management**: Schedule and manage shows, including details and cancellations.
- **Content Management**: Store and retrieve multimedia content, focusing on images.
- **Messaging and Notifications**: Utilize MassTransit for messaging and an event-driven architecture.
- **Emailer Service**: Send email notifications for new show additions.
- **Indexer Service**: Maintain an up-to-date search index for shows.
- **Dispatcher Service**: Manage event dispatch across the system.
- **Web Interface**: ASP.NET Core MVC-based user interface for comprehensive event management.

## Technology Stack
- **.NET Core**: For robust backend development.
- **Entity Framework Core**: For efficient database operations.
- **MassTransit**: For reliable messaging and event-driven architecture.
- **RabbitMQ**: For asynchronous communication.
- **ASP.NET Core MVC**: For a dynamic and responsive web interface.

## Architecture
The system is designed to follow a microservices architecture, enhancing scalability and maintainability. It includes several key components:

- **Act, Venue, Show, and Content Modules**: Handling specific aspects of event management.
- **Messaging System**: Powered by MassTransit and RabbitMQ.
- **Web Interface**: Providing a management interface for users.

## Getting Started
(Note: These steps will be applicable once the project is complete)
1. **Clone the Repository**: `git clone https://github.com/vyom1611/CelebraTix.git`
2. **Install .NET SDK**: Ensure you have the latest .NET SDK installed.
3. **Setup the Database**: Follow the instructions in `setup.md`.
4. **Run the Application**: Navigate to the project directory and execute `dotnet run`.

