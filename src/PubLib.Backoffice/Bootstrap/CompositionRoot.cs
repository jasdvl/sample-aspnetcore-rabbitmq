using PubLib.Backoffice.WebApp.Services;
using PubLib.Backoffice.WebApp.Services.Consumer;
using PubLib.Backoffice.WebApp.Services.Publisher;
using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
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

        services.AddSingleton<MessageService>();
        services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();
        services.AddSingleton<ExchangeSetupService>();

        services.AddSingleton<IBookReservationChannelFactory, BookReservationChannelFactory>();
        services.AddSingleton<IMembershipApplicationChannelFactory, MembershipApplicationChannelFactory>();

        services.AddSingleton<BookOrderPublisher>();
        services.AddSingleton<BookOrderPublisherService>();

        services.AddSingleton<MembershipStatusConsumer>();
        services.AddSingleton<BookReservationConsumer>();

        services.AddHostedService<RabbitMQExchangeSetupService>();

        services.AddHostedService<MembershipStatusConsumerService>();
        services.AddHostedService<BookReservationConsumerService>();
    }
}
