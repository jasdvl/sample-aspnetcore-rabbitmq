# RabbitMQ ASP.NET Integration Sample

This repository demonstrates how to integrate RabbitMQ into ASP.NET applications using both message publishers and consumers. The solution includes multiple projects to simulate a real-world event-driven system with RabbitMQ as the message broker. Additionally, it showcases how to manage RabbitMQ configurations through a centralized `YAML` file embedded as a resource.

**Note:** This project is still under development. Features and functionalities may be added or changed in future updates.

## Project Overview

This solution simulates workflows in a public library and is divided into multiple projects:

### 1. **PubLib.Messaging.RabbitMQ**

   - This project defines RabbitMQ consumers and publishers.
   - The configuration is managed via an embedded `rabbitmq-config.yml` file, which contains settings for exchanges, queues, and bindings.
   - The configuration file can be easily integrated into other projects using `AddRabbitMQConfig()` for dependency injection.

#### Example of `rabbitmq-config.yml`:

```yaml
RabbitMQ:
    HostName: localhost
    Port: 5672
    UserName: guest
    Password: guest
    Exchanges:
        - Name: membership-status-exchange
          Type: topic
          Durable: true
          AutoDelete: false
          Publishers:
            - MembershipStatusPublisher
        - Name: book-order-exchange
          Type: topic
          Durable: true
          AutoDelete: false
          Publishers:
            - BookOrderPublisher
    Queues:
        - Name: membership-status-queue
          Durable: true
          Exclusive: false
          AutoDelete: false
          Consumers:
            - MembershipStatusConsumer
        - Name: book-reservation-queue
          Durable: true
          Exclusive: false
          AutoDelete: false
          Consumers:
            - BookReservationConsumer
        - Name: book-provision-queue
          Durable: true
          Exclusive: false
          AutoDelete: false
          Consumers:
            - BookProvisionConsumer
    Bindings:
        - Queue: membership-status-queue
          Exchange: membership-status-exchange
          BindingKey: "membership.status.*"
        - Queue: book-reservation-queue
          Exchange: book-order-exchange
          BindingKey: "book.reservation.*"
        - Queue: book-provision-queue
          Exchange: book-order-exchange
          BindingKey: "book.provision.*"
```

### 2. **publib.frontdesk.client (Angular)**

   - This Angular client allows users to apply for library membership and reserve books once registered.
   - Book reservations and other actions are communicated to the ASP.NET Core backend through a REST API.
   - The client also displays real-time notifications from the ASP.NET backend using SignalR.

### 3. **PubLib.FrontDesk.Server (ASP.NET Core)**

   - The backend for the Angular client, exposing REST endpoints to process membership applications and book reservations.
   - Publishes these requests as messages to RabbitMQ in JSON format.
   - Also contains a consumer that listens for notifications (e.g., book ready for pickup) from RabbitMQ and forwards them to the Angular client via SignalR.

### 4. **PubLib.BackOffice (Blazor)**

   - A Blazor Web App for managing library operations.
   - It allows the backoffice to notify the front desk (via RabbitMQ) when a reserved book is ready for pickup.
   - Contains RabbitMQ consumers that receive membership applications and book reservations from the front desk to process and display within the Blazor application.

## Centralized RabbitMQ Configuration

RabbitMQ configuration is managed via the `rabbitmq-config.yml` file embedded in the `PubLib.Messaging.RabbitMQ` project. The file defines exchanges, queues, bindings, and publishers/consumers. The configuration can be injected into other projects using the following extension method:

```csharp
public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddRabbitMQConfig(this IConfigurationBuilder builder)
    {
        return builder.AddYamlResource("PubLib.Messaging.RabbitMQ.rabbitmq-config.yml");
    }
}
```

This allows other projects to load the project specific RabbitMQ settings directly without worrying about resource paths.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/download/) for the Angular client
- [Docker](https://www.docker.com/) for running RabbitMQ
- [RabbitMQ](https://www.rabbitmq.com/download.html)

## Getting Started

### 1. Start the RabbitMQ Server

To start a RabbitMQ server instance using Docker, run the following command:

```bash
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.13-management
```

Alternatively, you can explore other installation options [here](https://www.rabbitmq.com/docs/download).

### 2. Clone the Repository

```bash
git clone https://github.com/jasdvl/demo-rabbitmq-aspnetcore.git
cd demo-rabbitmq-aspnetcore
```

### 3. Run the Projects

#### For the Front Desk (Angular & ASP.NET Backend)

Navigate to the Angular frontend:

```bash
cd publib.frontdesk.client
npm install
npm start
```

Start the ASP.NET backend for publishing messages to RabbitMQ:

```bash
cd PubLib.FrontDesk.Server
dotnet run
```

#### For the Back Office (Blazor Web App)

Start the Blazor Web App:

```bash
cd PubLib.BackOffice
dotnet run
```

### 4. Sending and Viewing Messages

- Use the **PubLib.FrontDesk.Client (Angular)** to send membership applications and book reservations via the REST API.
- These requests will be published to RabbitMQ.
- The **PubLib.BackOffice (Blazor)** will consume these messages and display them in real time.
- Additionally, the Backoffice can notify the Front Desk via RabbitMQ/REST when a book is ready for pickup.

## TODO List

- Fix compiler warnings
- Correct incorrect namespaces
- Add XML comments
- Replace events with Channel\<T\>
- Refactor code
