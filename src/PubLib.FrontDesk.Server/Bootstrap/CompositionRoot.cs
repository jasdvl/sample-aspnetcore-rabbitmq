using PubLib.Messaging.RabbitMQ.Clients.Connection;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Publisher.Membership;
using PubLib.Messaging.RabbitMQ.Configuration;
using PubLib.Messaging.RabbitMQ.Setup;
using PubLib.Server.Services;
using PubLib.Server.Services.Consumer;
using PubLib.Server.Services.Publisher;

namespace PubLib.FrontDesk.Server.Bootstrap;

public class CompositionRoot()
{
    public WebApplicationBuilder GetBuilder(WebApplicationOptions options)
    {
        var builder = WebApplication.CreateBuilder(options);
        builder.Configuration.AddRabbitMQConfig();

        RegisterRabbitMqServices(builder.Services, builder.Configuration);

        // Other services
        ConfigureWebApiServices(builder.Services);

        return builder;
    }

    private void RegisterRabbitMqServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMQOptions>(configuration.GetSection("RabbitMQ"));

        services.AddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();

        services.AddSingleton<ExchangeSetupService>();
        services.AddHostedService<RabbitMQExchangeSetupService>();

        services.AddSingleton<IBookProvisionChannelFactory, BookProvisionChannelFactory>();

        services.AddSingleton<BookProvisionConsumer>();

        services.AddSingleton<MembershipStatusPublisher>();
        services.AddSingleton<BookOrderPublisher>();

        services.AddSingleton<MembershipStatusPublisherService>();
        services.AddSingleton<BookOrderPublisherService>();

        services.AddHostedService<BookProvisionConsumerService>();
    }

    private void ConfigureWebApiServices(IServiceCollection services)
    {
        services.AddControllers();

        // About configuring Swagger/OpenAPI: https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSignalR();
        //services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .WithOrigins("https://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });
    }
}
