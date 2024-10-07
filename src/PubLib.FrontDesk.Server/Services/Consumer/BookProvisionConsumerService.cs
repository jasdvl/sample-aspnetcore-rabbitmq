using Microsoft.AspNetCore.SignalR;
using PubLib.FrontDesk.Server.Const;
using PubLib.FrontDesk.Server.Messaging;
using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;

namespace PubLib.Server.Services.Consumer
{
    public class BookProvisionConsumerService : BackgroundService
    {
        private readonly BookProvisionConsumer _consumer;

        private readonly IHubContext<MessageHub> _hubContext;

        private readonly ILogger<BookProvisionConsumerService> _logger;

        public BookProvisionConsumerService(
                                        BookProvisionConsumer consumer,
                                        IHubContext<MessageHub> hubContext,
                                        ILogger<BookProvisionConsumerService> logger)
        {
            _consumer = consumer;
            _hubContext = hubContext;
            _logger = logger;

            _consumer.BookReadyForPickup += OnBookReadyForPickup;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(BookProvisionConsumerService)}.");
            await _consumer.ConsumeAsync(stoppingToken);
        }

        private void OnBookReadyForPickup(object? sender, BookProvisionReceivedEventArgs e)
        {
            string message = $"{e.QueueName}: Book ready for pickup: {e.BookProvision.Book.Title}";

            _hubContext.Clients.Groups(SignalRGroups.FrontDesk).SendAsync("BookReadyForPickup", e.BookProvision);

            _logger.LogInformation(message);

        }
    }
}
