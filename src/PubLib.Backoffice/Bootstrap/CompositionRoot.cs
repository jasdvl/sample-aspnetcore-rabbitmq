using PubLib.Backoffice.WebApp.Services;
using PubLib.Backoffice.WebApp.Services.Consumer;
using PubLib.Backoffice.WebApp.Services.Publisher;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Membership;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.BookOrder;
using PubLib.Messaging.RabbitMQ.Configuration;
using PubLib.Messaging.RabbitMQ.Setup;
using PubLib.Server.Services;

namespace PubLib.Backoffice.Bootstrap;

public class CompositionRoot()
{
    public WebApplicationBuilder GetBuilder(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);
        builder.Configuration.AddRabbitMQConfig();

        RegisterRabbitMQServices(builder.Services, builder.Configuration);

        builder.Services.AddRazorComponents().AddInteractiveServerComponents();

        return builder;
    }

    private void RegisterRabbitMQServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));

        // Singleton Services
        // Created once for the entire lifetime of the application.
        // The same instance is reused for all requests and components.
        // Suitable for stateless services or global resources.
        // Must be implemented in a thread-safe manner.

        services.AddSingleton<MessageService>();
        services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        services.AddSingleton<ExchangeSetupService>();
        services.AddSingleton<BookOrderPublisher>();
        services.AddSingleton<BookOrderPublisherService>();

        services.AddSingleton<MembershipStatusConsumer>();
        services.AddSingleton<BookReservationConsumer>();

        // Scoped Services
        // Created once per request (HTTP request).
        // Each request gets its own instance of the service.
        // The instance lasts for the duration of the request.
        // Suitable for services that manage request-specific data or states.

        // Hosted Services: run background tasks or long-running operations 
        // They run independently of the request processing pipeline.

        services.AddHostedService<RabbitMQExchangeSetupService>();

        services.AddHostedService<MembershipStatusConsumerService>();
        services.AddHostedService<BookReservationConsumerService>();
    }
}
