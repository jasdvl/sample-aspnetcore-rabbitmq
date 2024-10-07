using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;

namespace PubLib.Backoffice.WebApp.Services.Consumer
{
    public class BookReservationConsumerService : BackgroundService
    {
        private readonly BookReservationConsumer _consumer;

        private readonly ILogger<BookReservationConsumerService> _logger;

        private readonly MessageService _messageService;

        public BookReservationConsumerService(
                                        BookReservationConsumer consumer,
                                        ILogger<BookReservationConsumerService> logger,
                                        MessageService messageService)
        {
            _consumer = consumer;
            _logger = logger;
            _messageService = messageService;

            _consumer.BookReserved += OnBookReserved;
        }

        private void OnBookReserved(object? sender, BookReservationReceivedEventArgs e)
        {
            string message = $"Book reserved: {e.BookOrder.Book.Title}.";
            _logger.LogInformation(message);

            _messageService.AddMessage(message, e.QueueName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(BookReservationConsumerService)}.");
            await _consumer.ConsumeAsync(stoppingToken);
        }
    }
}
