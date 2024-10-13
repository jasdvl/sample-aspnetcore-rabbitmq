# RabbitMQ ASP.NET Integration Sample

This repository demonstrates how to integrate RabbitMQ into ASP.NET applications by utilizing both message publishers and consumers, with a focus on configuring RabbitMQ for client-side usage. The solution includes multiple projects to simulate a real-world event-driven system with RabbitMQ as the message broker. Additionally, it showcases how to manage RabbitMQ client configurations through a centralized `YAML` file embedded as a resource.

**Note:** This project is still under development. Features and functionalities may be added or changed in future updates.
The codebase may not fully adhere to best practices and may undergo structural improvements in future updates.  
See the [TODO List](#todo-list) for more details on planned improvements.  
Please note that time constraints might delay updates to the TODO list.

## Project Overview

This solution simulates workflows in a public library and is divided into multiple projects:

### 1. **PubLib.Messaging.RabbitMQ**

- This project defines RabbitMQ consumers and publishers.
- The RabbitMQ client configuration is managed via an embedded `rabbitmq-config.yml` file, which contains settings for exchanges, queues, bindings, and publishers/consumers.  
  This setup is specifically for configuring the client-side interaction with RabbitMQ and does not involve server configuration.

  #### Example of the `rabbitmq-config.yml` file used for client configuration:

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

- Inject the configuration into other projects via the `AddRabbitMQConfig()` extension method:

    ```csharp
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder AddRabbitMQConfig(this IConfigurationBuilder builder)
        {
            return builder.AddYamlResource("PubLib.Messaging.RabbitMQ.rabbitmq-config.yml");
        }
    }
    ```

    This method enables other projects to directly load project-specific RabbitMQ settings without handling resource paths manually.

### 2. **publib.frontdesk.client (Angular)**

   - This Angular client allows users to apply for library membership and reserve books once registered.
   - Book reservations and other actions are communicated to the ASP.NET Core backend through a REST API.
   - The client also displays real-time notifications from the ASP.NET backend using SignalR.

### 3. **PubLib.FrontDesk.Server (ASP.NET Core)**

   - The backend for the Angular client, exposing REST endpoints to process membership applications and book reservations.
   - Publishes these requests as messages to RabbitMQ in JSON format.
   - Also contains a consumer that listens for notifications (e.g., book ready for pickup) from RabbitMQ and forwards them to the Angular client via SignalR.

### 4. **PubLib.BackOffice (Blazor)**

   - A Blazor Web App (interactive SSR) for managing library operations.
   - It allows the Back Office to notify the Front Desk (via RabbitMQ) when a reserved book is ready for pickup.
   - Contains RabbitMQ consumers that receive membership applications and book reservations from the Front Desk to process and display within the Blazor application.

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
git clone https://github.com/jasdvl/sample-aspnetcore-rabbitmq.git
cd demo-rabbitmq-aspnetcore
```

### 3. Run the Projects

Depending on whether you're running the solution in Visual Studio or from the command line, follow the steps below:

#### **3.1 Running the Solution in Visual Studio**

After downloading and opening the solution in Visual Studio, it is recommended to configure multiple startup projects to run the application efficiently.
While it's possible to start each project individually by setting it as the startup project, the most convenient approach is to configure multiple startup projects, so that all necessary services are launched simultaneously.
Follow these steps to set up the three relevant projects as startup projects.

**Steps to Configure Multiple Startup Projects in Visual Studio:**

1. **Open the Solution:**
   After downloading and extracting the solution from GitHub, open the `.sln` file in Visual Studio.

2. **Access Startup Project Settings:**
   - In **Solution Explorer**, right-click on the solution node (at the top).
   - Select **Set Startup Projects...** from the context menu.

3. **Select Multiple Startup Projects:**
   - In the dialog that appears, choose **Multiple startup projects**.
   - For each of the following projects, select **Start** from the drop-down menu next to the project name:
     - PubLib.BackOffice
     - PubLib.FrontDesk.Server
     - publib.frontdesk.client

4. **Save Changes:**
   Click **OK** to save your changes.

Now, all three projects will be configured to start together when you run the solution.

**Note:** If you're unfamiliar with how to set multiple startup projects in Visual Studio, or you're running the solution for the first time, refer to the instructions below.  
For additional help, you can also check the official [Visual Studio documentation](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-set-multiple-startup-projects?view=vs-2022).

---

#### **3.2 Running the Solution from the Command Line**

You can also run the projects manually from the command line. Follow the instructions below to run each part of the application.

**1. Running the Back Office (Blazor Web App)**

Navigate to the Blazor Web App directory and run the application:

```bash
cd PubLib.BackOffice
dotnet run
```

**2. Running the Front Desk (Angular & ASP.NET Backend)**

First, start the ASP.NET backend for publishing messages to RabbitMQ:

```bash
cd PubLib.FrontDesk.Server
dotnet run
```

Then, start the Angular frontend by navigating to the client directory and running the following commands:

```bash
cd publib.frontdesk.client
npm install
npm start
```

**3. Sending and Viewing Messages**

- Use the **PubLib.FrontDesk.Client** to send membership applications and book reservations via the REST API.
- These requests will be published to RabbitMQ.
- The **PubLib.BackOffice** will consume these messages and display them in real time.
- The **PubLib.BackOffice** can also notify the Front Desk when a reserved book is ready for pickup.
  This notification is sent via RabbitMQ and then forwarded as a SignalR message from the **PubLib.FrontDesk.Server** to the Angular client.


## TODO List

### Priority 1

- Fix compiler warnings
- Correct or adjust inconsistent or incorrect namespaces
- Add XML comments
- ~~Replace events with Channel\<T\> (superior asynchronous messaging with built-in backpressure and concurrent producer-consumer support.)~~

### Priority 2

- Refactor code

### Priority 3

These items are considered "nice to have" and would enhance the system's capabilities. However, I’m not sure when I will be able to address them due to time constraints:

- Implement RPC (Remote Procedure Call) pattern for request/response handling
- Add message prioritization to queues
- Implement Dead Letter Queues (DLQ) for failed message handling
- Add message TTL (Time-to-Live) for automatic message expiration
- Implement delayed messaging for deferred processing
- Use Quorum Queues for higher availability and reliability

## Branching Strategy

Since I am the sole developer on this project, I primarily work on the `main` branch. I prefer to keep things simple by committing directly to `main` for most tasks. However, if a new feature requires multiple related commits or substantial changes, I will create feature branches to manage those updates. Once the feature is complete, the branch will be merged back into `main`. My goal is to keep the main branch stable and up to date.
