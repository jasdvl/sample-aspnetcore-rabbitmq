using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using System.Threading.Channels;

namespace PubLib.Backoffice.WebApp.Services.Consumer
{
    public class BookReservationConsumerService : BackgroundService
    {
        private readonly ILogger<BookReservationConsumerService> _logger;

        private readonly BookReservationConsumer _consumer;

        private readonly Channel<BookReservationReceivedEventArgs> _bookReservationChannel;

        private readonly MessageService _messageService;

        public BookReservationConsumerService(
                                            ILogger<BookReservationConsumerService> logger,
                                            BookReservationConsumer consumer,
                                            IBookReservationChannelFactory bookReservationChannelFactory,
                                            MessageService messageService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));
            _bookReservationChannel = bookReservationChannelFactory.GetChannel();
            _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting {ServiceName}.", nameof(BookReservationConsumerService));

            try
            {
                await Task.WhenAll(
                    _consumer.ConsumeAsync(stoppingToken),
                    ProcessBookReservationsAsync(stoppingToken)
                );
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("BookReservationConsumerService is stopping.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the consumer service.");
            }
        }

        private async Task ProcessBookReservationsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var reservation in _bookReservationChannel.Reader.ReadAllAsync(cancellationToken))
                {
                    HandleReservationAsync(reservation);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Processing reservations was canceled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing reservations.");
            }
        }

        private void HandleReservationAsync(BookReservationReceivedEventArgs reservation)
        {
            try
            {
                string message = $"Book reserved: {reservation.BookOrder.Book.Title}.";
                _logger.LogInformation(message);
                _messageService.AddMessage(message, reservation.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling a book reservation.");
                throw;
            }
        }
    }
}
