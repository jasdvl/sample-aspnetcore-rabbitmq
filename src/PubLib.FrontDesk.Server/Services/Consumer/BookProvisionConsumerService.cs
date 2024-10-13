using Microsoft.AspNetCore.SignalR;
using PubLib.FrontDesk.Server.Const;
using PubLib.FrontDesk.Server.Messaging;
using PubLib.Messaging.RabbitMQ.Clients.Consumer;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.BookOrder;
using PubLib.Messaging.RabbitMQ.Clients.Consumer.Channels;
using System.Threading.Channels;

namespace PubLib.Server.Services.Consumer
{
    public class BookProvisionConsumerService : BackgroundService
    {
        private readonly ILogger<BookProvisionConsumerService> _logger;

        private readonly BookProvisionConsumer _consumer;

        private readonly IHubContext<MessageHub> _hubContext;

        private readonly IBookProvisionChannelFactory _bookProvisionChannelFactory;

        private readonly Channel<BookProvisionReceivedEventArgs> _bookProvisionChannel;

        public BookProvisionConsumerService(
                                        ILogger<BookProvisionConsumerService> logger,
                                        BookProvisionConsumer consumer,
                                        IBookProvisionChannelFactory bookProvisionChannelFactory,
                                        IHubContext<MessageHub> hubContext)
        {
            _logger = logger;
            _consumer = consumer;
            _hubContext = hubContext;
            _bookProvisionChannelFactory = bookProvisionChannelFactory;
            _bookProvisionChannel = bookProvisionChannelFactory.GetChannel();
        }

        public override void Dispose()
        {
            _bookProvisionChannelFactory.Dispose();
            _consumer.Dispose();
            base.Dispose();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting {nameof(BookProvisionConsumerService)}.");
            await Task.WhenAll(_consumer.ConsumeAsync(stoppingToken), ProcessBookProvisionsAsync(stoppingToken));
        }

        private async Task ProcessBookProvisionsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await foreach (var reservation in _bookProvisionChannel.Reader.ReadAllAsync(cancellationToken))
                {
                    HandleBookProvisionAsync(reservation);
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

        private void HandleBookProvisionAsync(BookProvisionReceivedEventArgs reservation)
        {
            try
            {
                string message = $"Book ready for pickup: {reservation.BookProvision.Book.Title}.";
                _hubContext.Clients.Groups(SignalRGroups.FrontDesk).SendAsync("BookReadyForPickup", reservation.BookProvision);
                _logger.LogInformation(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while handling a book provision.");
                throw;
            }
        }
    }
}
